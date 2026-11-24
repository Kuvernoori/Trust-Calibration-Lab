import { StrictMode, useEffect, useState } from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter, useLocation, useNavigate } from 'react-router-dom'
import { getCases, getComments, getFile, getFiles, type CaseFile, type CaseSummary, type ReviewComment } from './api/cases'
import './styles.css'

function Shell({ children }: { children: React.ReactNode }) { return <div className="app-frame"><header className="topbar"><div className="brand"><span className="brand-mark">✦</span> trust<span className="accent">/</span>calibration <span className="tag">LAB</span></div><div className="topbar-meta"><span className="live-dot" /> DEMO WORKSPACE <span className="avatar">TC</span></div></header>{children}</div> }
function Catalog() {
  const navigate = useNavigate(); const [items, setItems] = useState<CaseSummary[]>([]); const [error, setError] = useState('')
  useEffect(() => { getCases().then(setItems).catch(() => setError('The case catalog could not be loaded. Is the API running?')) }, [])
  return <Shell><main className="catalog"><div className="catalog-hero"><div><div className="eyebrow">SECURE SOFTWARE DEVELOPMENT / PHASE 02</div><h1>Choose your<br /><span>review surface.</span></h1><p>Study the code. Question the comment. Calibrate your trust.</p></div><div className="catalog-stat"><strong>03</strong><span>educational<br />case files</span></div></div>{error ? <div className="state-card">{error}</div> : items.length === 0 ? <div className="state-card"><span className="spinner" /> Loading case files…</div> : <div className="catalog-grid">{items.map((item, index) => <button className="catalog-card" key={item.id} onClick={() => navigate(`/cases/${item.id}`)}><div className="card-top"><span className="case-label">CASE 0{index + 1}</span><span className="arrow">↗</span></div><h2>{item.title}</h2><p>{item.description}</p><div className="card-footer"><span className="difficulty-pill">{item.difficulty}</span><span>{item.vulnerabilityCount} vulnerabilities</span><span>{item.commentCount} AI comments</span></div><div className="hint-line"><span className="hint-icon">◌</span>{item.hintLevel === 'None' ? 'No hints — trust your instincts' : item.hintLevel === 'CategoryOnly' ? 'Category hints available' : 'Hints + example verdict'}</div></button>)}</div>}</main></Shell>
}
function Workspace() {
  const location = useLocation(); const navigate = useNavigate(); const caseId = Number(location.pathname.split('/')[2]); const [caseData, setCaseData] = useState<CaseSummary>(); const [files, setFiles] = useState<CaseFile[]>([]); const [comments, setComments] = useState<ReviewComment[]>([]); const [activeFile, setActiveFile] = useState<CaseFile>(); const [activeComment, setActiveComment] = useState<ReviewComment>(); const [error, setError] = useState('')
  useEffect(() => {
    let cancelled = false
    setError('')
    setCaseData(undefined)
    setActiveFile(undefined)
    Promise.all([getCases(), getFiles(caseId), getComments(caseId)])
      .then(async ([all, loadedFiles, loadedComments]) => {
        const found = all.find(item => item.id === caseId)
        if (!found || !loadedFiles[0]) throw new Error('Case data is incomplete')
        const firstFile = await getFile(caseId, loadedFiles[0].id)
        if (cancelled) return
        setCaseData(found); setFiles(loadedFiles); setComments(loadedComments); setActiveFile(firstFile)
      })
      .catch((reason: unknown) => { if (!cancelled) setError(reason instanceof Error ? reason.message : 'API request failed') })
    return () => { cancelled = true }
  }, [caseId])
  const selectComment = (comment: ReviewComment) => { setActiveComment(comment); const file = files.find(item => item.filePath === comment.filePath); if (file) getFile(caseId, file.id).then(setActiveFile) }
  if (error) return <Shell><div className="state-card workspace-state"><strong>This case could not be loaded.</strong><span>{error}</span><button onClick={() => navigate('/')}>Back to cases</button></div></Shell>
  if (!caseData || !activeFile) return <Shell><div className="state-card workspace-state"><span className="spinner" /> Loading workspace…</div></Shell>
  const lines = (activeFile.content ?? '').split('\n')
  return <Shell><div className="workspace-page"><div className="workspace-toolbar"><button className="back-button" onClick={() => navigate('/')}>← CASES</button><div><span className="toolbar-id">CASE 0{caseId}</span><strong>{caseData.title}</strong></div><span className="read-only">READ ONLY</span></div><div className="workspace-grid"><aside className="file-panel"><div className="panel-heading">EXPLORER <span>⌁</span></div><div className="folder">⌄ <strong>TRUST-CALIBRATION</strong></div><div className="folder-child">⌄ <strong>{caseId === 3 ? 'src' : 'project'}</strong></div>{files.map(file => <button className={`file-row ${activeFile.id === file.id ? 'active' : ''}`} key={file.id} onClick={() => getFile(caseId, file.id).then(setActiveFile)}>▤ <span>{file.filePath.split('/').pop()}</span></button>)}</aside><section className="code-panel"><div className="code-tab">{activeFile.filePath} <span> C#</span></div><div className="code-view">{lines.map((line, index) => <div className={`code-line ${activeComment?.filePath === activeFile.filePath && activeComment.lineNumber === index + 1 ? 'highlight' : ''}`} key={index}><span className="line-no">{index + 1}</span><code>{line || ' '}</code>{activeComment?.filePath === activeFile.filePath && activeComment.lineNumber === index + 1 && <span className="line-marker">●</span>}</div>)}</div></section><aside className="comments-panel"><div className="panel-heading">AI REVIEW <span>{comments.length} COMMENTS</span></div>{comments.map(comment => <button className={`comment-row ${activeComment?.id === comment.id ? 'active' : ''}`} key={comment.id} onClick={() => selectComment(comment)}><div className="comment-id">AI-{String(comment.id).slice(-2)} <span>›</span></div><strong>{comment.filePath.split('/').pop()} <em>:{comment.lineNumber}</em></strong><p>{comment.text}</p></button>)}</aside></div></div></Shell>
}
function App() {
  const location = useLocation()
  return location.pathname.startsWith('/cases/') ? <Workspace /> : <Catalog />
}
createRoot(document.getElementById('root')!).render(<StrictMode><BrowserRouter><App /></BrowserRouter></StrictMode>)
