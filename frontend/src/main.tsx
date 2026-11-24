import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter } from 'react-router-dom'
import './styles.css'
import { getHealth } from './api/health'

function App() {
  const checkApi = async () => {
    const health = await getHealth()
    alert(`${health.service}: ${health.status}`)
  }

  return (
    <main className="app-shell">
      <h1>Trust Calibration Lab</h1>
      <p>Foundation ready. Learning activities will be added incrementally.</p>
      <button type="button" onClick={checkApi}>Check API health</button>
    </main>
  )
}

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <BrowserRouter>
      <App />
    </BrowserRouter>
  </StrictMode>,
)
