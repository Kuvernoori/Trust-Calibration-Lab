import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter } from 'react-router-dom'
import './styles.css'

function App() {
  return (
    <main className="app-shell">
      <h1>Trust Calibration Lab</h1>
      <p>Foundation ready. Learning activities will be added incrementally.</p>
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
