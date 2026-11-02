import { useState } from 'react'
import AuthFormField from '../components/AuthFormField'
import LogoMark from '../../../shared/components/LogoMark'
import './AuthPage.css'

function AuthPage() {
  const [mode, setMode] = useState('login')
  const isLogin = mode === 'login'

  const handleSubmit = (event) => {
    event.preventDefault()
  }

  return (
    <main className="auth-shell">
      <aside className="brand-panel">
        <div className="brand">
          <LogoMark />
          <span>Swift<span className="brand-accent">Track</span></span>
        </div>

        <div className="brand-content">
          <p className="eyebrow">Move smarter. Deliver faster.</p>
          <h1>Everything you need to keep your business moving.</h1>
          <p className="brand-description">
            Track your operations, connect your team, and stay ahead of every delivery with one simple workspace.
          </p>
        </div>

        <div className="brand-footer">
          <span className="status-dot" />
          <span>Trusted by teams that move things forward</span>
        </div>
      </aside>

      <section className="form-panel">
        <div className="form-wrap">
          <div className="mobile-brand brand">
            <LogoMark />
            <span>Swift<span className="brand-accent">Track</span></span>
          </div>

          <div className="form-heading">
            <p className="eyebrow">{isLogin ? 'Welcome back' : 'Get started today'}</p>
            <h2>{isLogin ? 'Log in to your account' : 'Create your account'}</h2>
            <p>{isLogin ? 'Enter your details to access your workspace.' : 'Join SwiftTrack and make every move count.'}</p>
          </div>

          <div className="auth-switcher" role="tablist" aria-label="Authentication options">
            <button type="button" className={isLogin ? 'active' : ''} onClick={() => setMode('login')} role="tab" aria-selected={isLogin}>Log in</button>
            <button type="button" className={!isLogin ? 'active' : ''} onClick={() => setMode('signup')} role="tab" aria-selected={!isLogin}>Sign up</button>
          </div>

          <form onSubmit={handleSubmit}>
            {isLogin ? (
              <>
                <AuthFormField label="Email address" type="email" name="email" placeholder="you@company.com" />
                <div className="password-label">
                  <span>Password</span>
                  <a href="#forgot-password">Forgot password?</a>
                </div>
                <input className="password-input" type="password" name="password" placeholder="Enter your password" required />
              </>
            ) : (
              <div className="signup-fields">
                <div className="field-row">
                  <AuthFormField label="First name" name="firstName" placeholder="Alex" />
                  <AuthFormField label="Last name" name="lastName" placeholder="Morgan" />
                </div>
                <AuthFormField label="Email address" type="email" name="email" placeholder="you@company.com" />
                <AuthFormField label="NIC number" name="nic" placeholder="Enter your NIC number" />
                <AuthFormField label="Address" name="address" placeholder="Street address, city" />
                <div className="field-row">
                  <AuthFormField label="Password" type="password" name="password" placeholder="Create a password" />
                  <AuthFormField label="Confirm password" type="password" name="confirmPassword" placeholder="Repeat password" />
                </div>
              </div>
            )}

            {!isLogin && (
              <label className="terms">
                <input type="checkbox" required />
                <span>I agree to the <a href="#terms">Terms of Service</a> and <a href="#privacy">Privacy Policy</a>.</span>
              </label>
            )}

            <button className="submit-button" type="submit">{isLogin ? 'Log in to SwiftTrack' : 'Create SwiftTrack account'} <span aria-hidden="true">→</span></button>
          </form>

          <p className="form-footnote">
            {isLogin ? <>New to SwiftTrack? <button type="button" onClick={() => setMode('signup')}>Create an account</button></> : <>Already have an account? <button type="button" onClick={() => setMode('login')}>Log in</button></>}
          </p>
        </div>
      </section>
    </main>
  )
}

export default AuthPage
