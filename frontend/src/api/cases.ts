export type CaseSummary = { id: number; title: string; description: string; difficulty: string; vulnerabilityCount: number; commentCount: number; hintLevel: string }
export type CaseFile = { id: number; caseId: number; filePath: string; content?: string }
export type ReviewComment = { id: number; caseId: number; filePath: string; lineNumber: number; text: string }
export type ReviewAnswer = { id: number; reviewCommentId: number; verdict: string; confidence: string; evidenceType: string; evidenceText: string }
export type Submission = { id: number; caseId: number; status: 'Draft' | 'Submitted'; answers: ReviewAnswer[] }
export type ReviewResult = { id: number; caseId: number; available: boolean; submittedAnswers: number; totalComments: number; correct?: number; incorrect?: number; partiallyCorrect?: number; accuracy?: number; highConfidenceErrorRate: number | null; correctFindings?: number; totalFindings?: number; lo1?: boolean; lo2?: boolean; confidenceSummary?: Array<{ level: string; total: number; correct: number; incorrect: number }>; findings?: Array<Finding>; answers?: Array<ReviewAnswer & { correctVerdict: string; isCorrect: boolean; explanation: string; comment: string }> }
export type Finding = { id: number; title: string; cwe: string; filePath: string; lineNumber?: number; explanation: string; evidenceText: string; source: string; matched?: boolean }
const apiBaseUrl = (import.meta.env.VITE_API_BASE_URL ?? '').replace(/\/$/, '')
const request = async <T,>(path: string, init?: RequestInit): Promise<T> => { const response = await fetch(`${apiBaseUrl}${path}`, { headers: { 'Content-Type': 'application/json' }, ...init }); if (!response.ok) { const detail = await response.text(); throw new Error(detail || `Request failed ${response.status}: ${path}`) } return response.json() as Promise<T> }
export const getCases = () => request<CaseSummary[]>('/api/cases')
export const getFiles = (id: number) => request<CaseFile[]>(`/api/cases/${id}/files`)
export const getComments = (id: number) => request<ReviewComment[]>(`/api/cases/${id}/comments`)
export const getFile = (caseId: number, fileId: number) => request<CaseFile>(`/api/cases/${caseId}/files/${fileId}`)
export const createSubmission = (caseId: number, sessionId: string) => request<Submission>('/api/submissions', { method: 'POST', body: JSON.stringify({ caseId, sessionId }) })
export const getSubmission = (id: number) => request<Submission>(`/api/submissions/${id}`)
export const saveAnswer = (submissionId: number, commentId: number, answer: Omit<ReviewAnswer, 'id' | 'reviewCommentId'>) => request<Submission>(`/api/submissions/${submissionId}/answers/${commentId}`, { method: 'PUT', body: JSON.stringify(answer) })
export const submitReview = (id: number) => request<Submission>(`/api/submissions/${id}/submit`, { method: 'POST' })
export const getResults = (id: number) => request<ReviewResult>(`/api/submissions/${id}/results`)
export const addFinding = (id: number, finding: Omit<Finding, 'id' | 'matched'>) => request<Finding>(`/api/submissions/${id}/findings`, { method: 'POST', body: JSON.stringify(finding) })
export const deleteFinding = (id: number, findingId: number) => request<void>(`/api/submissions/${id}/findings/${findingId}`, { method: 'DELETE' })
