# Trust Calibration Lab

Educational web application for evaluating AI-generated secure code review comments.

## Project structure

- `frontend/` — React, TypeScript, Vite, and React Router client
- `backend/TrustCalibrationLab.Api/` — ASP.NET Core Web API foundation targeting .NET 10

## Run locally

### Dependencies

- Node.js 20 or newer and npm.
- .NET 10 SDK.
- SQLite is provided through the `Microsoft.EntityFrameworkCore.Sqlite` NuGet package; no separate SQLite server is required.

Check installations:

```powershell
node --version
npm --version
dotnet --version
```

Run the backend and frontend in two separate terminals.

```powershell
cd frontend
npm install
npm run dev
```

```powershell
cd backend/TrustCalibrationLab.Api
dotnet run
```

Open `http://localhost:5173`. The API runs at `http://localhost:5070`. On the first backend start, SQLite creates `trust-calibration-lab.db` and seeds the demonstration cases automatically.

The frontend uses Vite's HTTP proxy to forward `/api/*` requests to `http://localhost:5070`. The API exposes `GET /api/health` and allows local development origins `http://localhost:5173` and `http://localhost:4173`.

## Phase 2 API

- `GET /api/cases` — student-safe case catalog metadata
- `GET /api/cases/{id}` — one case summary
- `GET /api/cases/{id}/files` — file explorer entries
- `GET /api/cases/{id}/files/{fileId}` — read-only file content
- `GET /api/cases/{id}/comments` — AI review comments

The SQLite database is initialized and seeded automatically on API startup. Case 1 and Case 2 contain clearly marked demonstration content; Case 3 contains the specified vulnerability and comment dataset. Correct answers are kept in a backend-only answer key and are not returned by student-facing endpoints.
