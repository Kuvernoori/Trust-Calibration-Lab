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

The frontend uses Vite's HTTP proxy to forward `/api/*` requests to `http://localhost:5070`. The API exposes `GET /api/health` and allows local development origins `http://localhost:5173` and `http://localhost:4173`.
