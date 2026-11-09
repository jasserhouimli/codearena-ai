# 🚀 CodeArena AI — Intelligent Multi-Stage Technical Recruitment & Code Defense Platform

> An enterprise-grade recruitment and technical screening platform built to eliminate spam applications and automate developer vetting using GitHub repository analysis, AI-generated project-defense quizzes, and live coding sandboxes with keystroke replay analytics.

---

## 🌟 Overview

Traditional hiring platforms rely on self-reported CVs and unverified text profiles. **CodeArena AI** solves this by establishing a **4-stage automated verification pipeline** that connects directly to developer portfolios, evaluates actual codebase quality, quizzes candidates on their own implementation decisions, and provides recruiters with a video-like keystroke replay inspector.

---

## 🏗️ Architecture & Core Tech Stack

```
                       ┌──────────────────────────────────────────────┐
                       │            React + TypeScript UI             │
                       │  (Monaco Editor, Tailwind CSS, SignalR)      │
                       └──────────────────────┬───────────────────────┘
                                              │ REST API & WebSockets
                                              ▼
                       ┌──────────────────────────────────────────────┐
                       │              .NET 8 / 9 Web API              │
                       │    (Controllers, EF Core, JWT, SignalR)      │
                       └──────┬───────────────────────┬───────────────┘
                              │                       │
      Asynchronous Queue      │                       │ OAuth / API Requests
   ┌──────────────────────────┘                       └─────────────────────────┐
   ▼                                                                            ▼
┌─────────────────────────────────┐   ┌──────────────────────────────┐   ┌──────────────────┐
│     .NET Background Service     │   │     PostgreSQL (Neon/Sup.)   │   │ GitHub REST API  │
│ (AI Prompts & Pipeline Workers) │   │ (Users, Apps, Replays, Jobs) │   │ & Gemini 1.5 API │
└─────────────────────────────────┘   └──────────────────────────────┘   └──────────────────┘
```

* **Frontend:** React, TypeScript, Tailwind CSS, Monaco Editor (`@monaco-editor/react`), Lucide Icons, Microsoft SignalR Client.
* **Backend:** ASP.NET Core 8/9 Web API, Entity Framework Core, ASP.NET Core Identity / JWT Authentication.
* **Database:** Managed PostgreSQL (Hosted on Neon.tech / Supabase).
* **AI & Intelligence:** Google Gemini 1.5 Flash API (Structured JSON generation for CV parsing, code audits, and dynamic quiz generation).
* **Hosting Stack ($0/mo):** Azure App Service (Backend F1 Tier) + Vercel (Frontend CDN) + Neon.tech (Database).

---

## 🔄 The 4-Stage Gated Recruitment Pipeline

1. **Stage 1: Portfolio & Repository Audit**
   * Candidate connects via **GitHub OAuth 2.0**.
   * System fetches public repositories, language distribution, and commits via GitHub API.
   * AI parses uploaded CV (PDF) and cross-references claimed skills against actual repository code.
2. **Stage 2: AI "Code Defense" Quiz**
   * The AI reviews the candidate's actual repository structure and generates 3–5 targeted technical defense questions (e.g., *"In repo X, you used JWT tokens with cookie storage; why did you choose HttpOnly over localStorage?"*).
   * Candidate submits answers for instant AI evaluation.
3. **Stage 3: Live Coding Sandbox & Keystroke Replay**
   * Candidate solves a 15-minute coding challenge inside the **Monaco Editor**.
   * A lightweight event listener records temporal keystrokes (`timestamp`, `cursor`, `diffs`, `pasteCount`).
4. **Stage 4: Recruiter War Room & Kanban Review**
   * Recruiter views candidates on a real-time Kanban board (powered by SignalR).
   * Recruiter opens the candidate profile to inspect the **Keystroke Replay Player** (Play, Pause, Scrub timeline, Paste Alert markers).

---

## 👥 Role-Based Access Control (RBAC)

* **Candidate (Developer):**
  * OAuth login with GitHub.
  * Browse AI-recommended jobs with match confidence scores.
  * Progress through the 4-stage automated assessment workflow.
* **Client / Recruiter:**
  * Post job descriptions and skill weights.
  * Monitor incoming candidate pipeline via live Kanban board.
  * Review code audit metrics, AI quiz scores, and watch live keystroke replays.
* **Platform Admin:**
  * Manage global skill taxonomies, audit logs, and system monitoring.

---

## 🗄️ Core Database Schema (Entity Framework Core)

* **Users / UserProfiles:** Stores authentication details, GitHub access tokens, and parsed skill tags.
* **JobPosts:** Stores enterprise job listings, required tech stacks, and salary/budget ranges.
* **Applications:** Tracks candidate progression across the 4 gated pipeline stages.
* **DefenseQuestions:** Stores AI-generated repository-specific interview questions and candidate answers.
* **AssessmentReplays:** Stores raw keystroke event JSON arrays and time-series diff metrics for code review playback.

---

## 🚀 Getting Started & Deployment ($0 Free Tier Stack)

1. **Clone the Repository:**
   ```bash
   git clone https://github.com/your-username/code-arena-ai.git
   ```
2. **Configure Environment Variables:**
   * Backend (`appsettings.json`): Set PostgreSQL connection string (Neon/Supabase) and Gemini API Key.
   * Frontend (`.env`): Set `VITE_API_BASE_URL` and SignalR Hub endpoints.
3. **Run Migrations & Launch:**
   * Backend: `dotnet ef database update` -> `dotnet run`
   * Frontend: `npm install` -> `npm run dev`
4. **Deploy Free:**
   * Push backend to Azure App Service (F1 Plan) and frontend to Vercel. Connect your Neon PostgreSQL instance via environment variables.

---

## 💡 Why This Project Stands Out for PFE & Interviews
* **Solves a Real-World Problem:** Eliminates resume fraud and hiring bottlenecks for engineering teams.
* **Advanced Engineering Patterns:** Utilizes JWT RBAC, Entity Framework Core relational mappings, asynchronous background queues, and real-time WebSockets (SignalR).
* **Unique Innovation:** Features a custom keystroke replay analytics engine and portfolio-based AI code-defense questioning.