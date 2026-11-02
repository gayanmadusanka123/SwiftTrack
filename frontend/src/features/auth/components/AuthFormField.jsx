function AuthFormField({ label, type = 'text', placeholder, name, required = true }) {
  return (
    <label className="field">
      <span>{label}</span>
      <input type={type} name={name} placeholder={placeholder} required={required} />
    </label>
  )
}

export default AuthFormField
