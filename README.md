# Trust Calibration Lab

Educational web application for evaluating AI-generated secure code review comments.

## Project structure

- `frontend/` — React, TypeScript, Vite, and React Router client
- `backend/TrustCalibrationLab.Api/` — ASP.NET Core Web API foundation targeting .NET 10

## Run locally

```powershell
cd frontend
npm install
npm run dev
```

```powershell
cd backend/TrustCalibrationLab.Api
dotnet run
```

The API exposes a minimal health endpoint at `GET /api/health`.
