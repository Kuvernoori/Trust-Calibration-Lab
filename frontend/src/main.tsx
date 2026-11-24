import { StrictMode, useState } from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter } from 'react-router-dom'
import './styles.css'

type Case = { id: string; title: string; language: string; difficulty: string; category: string; code: string; comment: string; verdict: string; rationale: string }
const cases: Case[] = [
  { id: 'SEC-042', title: 'The convenient redirect', language: 'C#', difficulty: 'Intermediate', category: 'Input validation', code: 'public IActionResult Go(string returnUrl)\n{\n    return Redirect(returnUrl);\n}', comment: 'This endpoint is vulnerable to an open redirect. An attacker can craft a link that sends users to a malicious domain after authentication.', verdict: 'Valid', rationale: 'The URL is never constrained to a local path. Validate with Url.IsLocalUrl or use a server-side allowlist before redirecting.' },
  { id: 'SEC-017', title: 'A quiet SQL query', language: 'Python', difficulty: 'Intro', category: 'Injection', code: 'query = "SELECT * FROM users WHERE id = ?"\nuser = db.execute(query, (user_id,)).fetchone()', comment: 'String interpolation here allows an attacker to inject SQL through user_id and access arbitrary records.', verdict: 'False positive', rationale: 'The parameter placeholder and tuple of bound values keep user_id out of the SQL syntax. This is a parameterized query.' },
  { id: 'SEC-031', title: 'Remember me, maybe', language: 'TypeScript', difficulty: 'Advanced', category: 'Session security', code: 'res.cookie("session", token, {\n    httpOnly: true,\n    maxAge: 2_592_000_000\n});', comment: 'The session cookie should also set Secure and SameSite attributes. Without them, session theft and CSRF become more likely.', verdict: 'Partially valid', rationale: 'HttpOnly helps against script access, but a long-lived cookie should also use Secure and an appropriate SameSite policy.' },
]

function App() {
  const [activeId, setActiveId] = useState(cases[0].id)
  const [decisions, setDecisions] = useState<Record<string, string>>({})
  const [loading, setLoading] = useState(false)
  const [feedback, setFeedback] = useState<'correct' | 'incorrect' | null>(null)
  const activeCase = cases.find((item) => item.id === activeId) ?? cases[0]
  const completed = Object.keys(decisions).length
  const chooseDecision = (decision: string) => {
    setLoading(true); setFeedback(null)
    window.setTimeout(() => { setDecisions((current) => ({ ...current, [activeCase.id]: decision })); setFeedback(decision === activeCase.verdict ? 'correct' : 'incorrect'); setLoading(false) }, 420)
  }
  return <div className="app-frame">
    <header className="topbar"><div className="brand"><span className="brand-mark">✦</span><span>trust<span className="accent">/</span>calibration</span><span className="tag">LAB</span></div><div className="topbar-meta"><span className="live-dot" /> Prototype workspace <span className="avatar">SK</span></div></header>
    <div className="workspace">
      <aside className="sidebar"><div className="eyebrow">SECURE SOFTWARE DEVELOPMENT</div><h1>Review with<br /><span>intent.</span></h1><p className="intro">Build better instincts by calibrating your trust in AI-generated security reviews.</p>
        <div className="progress-card"><div className="progress-heading"><span>SESSION PROGRESS</span><strong>{completed}/{cases.length}</strong></div><div className="progress-track"><span style={{ width: `${completed / cases.length * 100}%` }} /></div><p>{completed === cases.length ? 'Session complete — nice work.' : `${cases.length - completed} reviews left in this session`}</p></div>
        <nav className="case-list" aria-label="Review cases"><div className="list-label">CASES <span>01—03</span></div>{cases.map((item, index) => <button className={`case-nav ${item.id === activeCase.id ? 'selected' : ''}`} key={item.id} onClick={() => { setActiveId(item.id); setFeedback(null) }}><span className="case-number">0{index + 1}</span><span className="case-info"><strong>{item.title}</strong><small>{item.category}</small></span><span className={`case-status ${decisions[item.id] ? 'done' : ''}`}>{decisions[item.id] ? '✓' : '›'}</span></button>)}</nav>
        <div className="sidebar-footer"><span className="footer-icon">◌</span><span>Trust is earned,<br />not assumed.</span></div>
      </aside>
      <main className="content"><div className="content-header"><div><div className="breadcrumb">CASE FILE <span>/</span> {activeCase.id}</div><h2>{activeCase.title}</h2></div><div className="difficulty"><span className="difficulty-dot" /> {activeCase.difficulty}</div></div><div className="case-meta"><span>{activeCase.category}</span><span className="separator">•</span><span>Review {cases.findIndex((item) => item.id === activeCase.id) + 1} of {cases.length}</span></div>
        <section className="editor-card"><div className="panel-bar"><span className="panel-title"><span className="file-icon">▤</span> auth.controller.{activeCase.language.toLowerCase()}</span><span className="code-context">READ ONLY <i>•••</i></span></div><div className="code-area"><div className="line-numbers">{activeCase.code.split('\n').map((_, i) => <span key={i}>{String(i + 1).padStart(2, '0')}</span>)}</div><pre><code>{activeCase.code}</code></pre></div></section>
        <section className="comment-card"><div className="comment-header"><div className="ai-badge">✦</div><div><strong>AI REVIEW COMMENT</strong><span>Generated analysis · security scan #24</span></div><span className="confidence">Confidence <b>86%</b></span></div><p className="comment-text">“{activeCase.comment}”</p></section>
        <section className="decision-section"><div className="section-kicker">YOUR ASSESSMENT</div><h3>How would you classify this comment?</h3><p className="decision-hint">Take a moment to inspect the code before deciding. There is no penalty for changing your mind.</p><div className="decision-grid">{['Valid', 'Partially valid', 'False positive'].map((option) => <button className={`decision-button ${decisions[activeCase.id] === option ? 'chosen' : ''}`} key={option} disabled={loading} onClick={() => chooseDecision(option)}><span className="radio">{decisions[activeCase.id] === option ? '●' : '○'}</span><span>{option}</span></button>)}</div>{loading && <div className="feedback loading"><span className="spinner" /> Comparing your assessment…</div>}{feedback && !loading && <div className={`feedback ${feedback}`}><span>{feedback === 'correct' ? '✓' : '↗'}</span><div><strong>{feedback === 'correct' ? 'Good calibration.' : 'Worth a second look.'}</strong><p>{activeCase.rationale}</p></div></div>}</section>
      </main>
    </div>
  </div>
}
createRoot(document.getElementById('root')!).render(<StrictMode><BrowserRouter><App /></BrowserRouter></StrictMode>)
