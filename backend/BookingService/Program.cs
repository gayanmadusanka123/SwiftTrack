using BookingService.ConnectedServices.implementations;
using BookingService.ConnectedServices.Interfaces;
using BookingService.Contracts;
using BookingService.Kafka.Configuration;
using BookingService.Kafka.Consumer;
using BookingService.Kafka.Publisher;
using BookingService.Model;
using BookingService.Services;
using CoreWCF;
using CoreWCF.Channels;
using CoreWCF.Configuration;
using CoreWCF.Description;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// db
builder.Services.AddDbContext<BookingDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("BookingDb")));

// Configure Kafka Options
builder.Services.Configure<KafkaOptions>(
    builder.Configuration.GetSection(KafkaOptions.SectionName));

// put the real port for them these are just mocks
builder.Services.AddHttpClient<IverificationClient, VerificationClient>(
    client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["ServiceUrls:VerificationService"] ?? "http://localhost:5006");
    }
);

builder.Services.AddHttpClient<IInventoryClient, InventoryClient>(
    client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["ServiceUrls:InventoryService"] ?? "http://localhost:5002");
    }
);

//Domain Services & CoreWCF Registration
builder.Services.AddScoped<PricingCalculator>();
builder.Services.AddTransient<BookingSoapService>();

builder.Services.AddServiceModelServices();
builder.Services.AddServiceModelMetadata();
builder.Services.AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();

//Kafka Event Handlers & Hosted Background Workers
builder.Services.AddScoped<PaymentSucceededConsumer>();
builder.Services.AddScoped<PaymentFailedConsumer>();
builder.Services.AddScoped<ReservationExpiredConsumer>();

builder.Services.AddHostedService<OutBoxPublisher>();
builder.Services.AddHostedService<KafkaConsumerWorker>();


var app = builder.Build();

// Configure CoreWCF SOAP Endpoint and WSDL Metadata
app.UseServiceModel(
    serviceBuilder =>
    {
        serviceBuilder.AddService<BookingSoapService>(ServiceOptions =>
    {
        ServiceOptions.DebugBehavior.IncludeExceptionDetailInFaults = true;
    });

        serviceBuilder.AddServiceEndpoint<BookingSoapService,IBookingSoapService>(
            new BasicHttpBinding(BasicHttpSecurityMode.None),
            "/BookingService.svc"
        );

        var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
        serviceMetadataBehavior.HttpGetEnabled = true;
        serviceMetadataBehavior.HttpsGetEnabled = true;
        }
);

app.Run();
