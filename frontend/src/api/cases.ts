export type CaseSummary = { id: number; title: string; description: string; difficulty: string; vulnerabilityCount: number; commentCount: number; hintLevel: string }
export type CaseFile = { id: number; caseId: number; filePath: string; content?: string }
export type ReviewComment = { id: number; caseId: number; filePath: string; lineNumber: number; text: string }
const request = async <T,>(path: string): Promise<T> => { const response = await fetch(path); if (!response.ok) throw new Error(`Request failed ${response.status}: ${path}`); return response.json() as Promise<T> }
export const getCases = () => request<CaseSummary[]>('/api/cases')
export const getFiles = (id: number) => request<CaseFile[]>(`/api/cases/${id}/files`)
export const getComments = (id: number) => request<ReviewComment[]>(`/api/cases/${id}/comments`)
export const getFile = (caseId: number, fileId: number) => request<CaseFile>(`/api/cases/${caseId}/files/${fileId}`)
