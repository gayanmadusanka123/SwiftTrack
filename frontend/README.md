# SwiftTrack frontend

The frontend is organized by application responsibility so new order-management
features can be added without coupling them to the authentication flow.

## Structure

```text
src/
├── app/                  # Application composition and root-level setup
├── features/
│   └── auth/             # Login and signup pages/components
├── shared/
│   ├── components/       # Reusable UI components
│   ├── hooks/            # Reusable React hooks
│   ├── services/         # API clients and integrations
│   └── utils/            # Shared pure helpers
├── assets/               # Images, icons, and other static imports
├── index.css             # Global styles and resets
└── main.jsx              # Browser entry point
```

Future modules such as `features/orders`, `features/customers`, and
`features/inventory` should keep their pages, components, hooks, and services
inside their own feature folder. Cross-feature code belongs in `shared/`.

This template provides a minimal setup to get React working in Vite with HMR and some Oxlint rules.

Currently, two official plugins are available:

- [@vitejs/plugin-react](https://github.com/vitejs/vite-plugin-react/blob/main/packages/plugin-react) uses [Oxc](https://oxc.rs)
- [@vitejs/plugin-react-swc](https://github.com/vitejs/vite-plugin-react/blob/main/packages/plugin-react-swc) uses [SWC](https://swc.rs/)

## React Compiler

The React Compiler is not enabled on this template because of its impact on dev & build performances. To add it, see [this documentation](https://react.dev/learn/react-compiler/installation).

## Expanding the Oxlint configuration

If you are developing a production application, we recommend using TypeScript with type-aware lint rules enabled. Check out the [TS template](https://github.com/vitejs/vite/tree/main/packages/create-vite/template-react-ts) for information on how to integrate TypeScript and Oxlint's TypeScript related rules in your project.
