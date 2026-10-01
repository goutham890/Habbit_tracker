# âš¡ Habit Tracker â€” Gamified Discipline & Annual Matrix

<div align="center">

![Habit Tracker Banner](icon256.png)

### **Track Today. Improve Everyday. Transform Your Life.**

[![Platform](https://img.shields.io/badge/platform-Windows%20%7C%20Web-blue.svg)](#)
[![Tech](https://img.shields.io/badge/stack-HTML5%20%7C%20CSS3%20%7C%20Vanilla%20JS%20%7C%20C%23-6366f1.svg)](#)
[![Offline](https://img.shields.io/badge/data-100%25%20Offline%20%26%20Private-emerald.svg)](#)
[![License](https://img.shields.io/badge/license-MIT-purple.svg)](LICENSE)

</div>

---

## ðŸŒŸ Overview

**Habit Tracker** is a high-performance, aesthetically pleasing gamified habit tracking desktop and web application. Designed with a **Martial Arts / Warrior's Path XP level-up system**, comprehensive **12-Month Annual Statistics**, an interactive **365-day consistency heatmap**, and high-contrast **Light & Obsidian Dark modes**.

Zero dependencies. Zero tracking. 100% local and private.

---

## âœ¨ Features

### ðŸ¥‹ Gamified Discipline Level System
- **Earn XP**: Receive **+10 XP** for every habit checked off each day.
- **Warrior's Path Ranks**:
  - **Level 1 â€“ 2**: APPRENTICE ðŸ¥‹ â€” Starting the discipline path
  - **Level 3 â€“ 4**: NOVICE âš¡ â€” Forging daily momentum
  - **Level 5 â€“ 6**: DISCIPLE âš”ï¸ â€” Unbreakable routine
  - **Level 7 â€“ 8**: WARRIOR ðŸ—¡ï¸ â€” Honed focus and execution
  - **Level 9 â€“ 10**: ELITE WARRIOR ðŸ‰ â€” Long-term consistency master
  - **Level 11 â€“ 13**: MASTER ðŸ›¡ï¸ â€” High-level habit mastery
  - **Level 14 â€“ 16**: GRANDMASTER ðŸ‘‘ â€” Unshakable discipline
  - **Level 17+**: IMMORTAL SAGE ðŸŒŒ â€” Habit immortality
- **Dynamic Status State**: Adapts in real time:
  - ðŸŒ± STATUS: WALKING THE PATH (< 50% completion)
  - âš¡ STATUS: DISCIPLINE OF IRON (50% â€“ 79% completion)
  - ðŸ”¥ STATUS: INNER FIRE AWAKENED (80%+ completion)

### ðŸ“Š Monthly Habit Matrix
- **31-Day Checkbox Grid** divided into Week 1 through Week 5 with vivid pastel themes.
- **Daily Line Graph Curve**: Real-time SVG graph tracking daily completion % throughout the month.
- **Weekly Progress Tubes**: 3D-styled fluid progress tubes visualizing completion rate per week.
- **Overall Donut Gauge**: Animated circular completion percentage meter.
- **Weekly Focus Routines**: Plan, manage, and check off weekly priorities and non-negotiables.

### ðŸ“ˆ Annual Statistics & 12-Month Overview
- **365-Day Consistency Heatmap**: GitHub-style annual matrix highlighting your daily dedication.
- **12 Month Cards Grid**: View checks, completion %, streaks, and top habits for each month.
- **Dedicated Month Navigation**: Jump instantly into any month's breakdown with the dedicated VIEW MONTH DETAILS â†’ button.
- **Quarterly Performance Comparison**: Side-by-side breakdown across Q1, Q2, Q3, and Q4.
- **12-Month Trajectory Curve**: Smooth multi-point SVG chart displaying year-long consistency trends.

### ðŸ“… Live Calendar Year Synchronization
- Detects the current real-world calendar year automatically.
- Automatically rolls forward into new calendar years (2026, 2027, 2028+) without losing historical entries.
- Dropdown range dynamically supports years 2026 through 2050+.

### ðŸŒ“ Ultra-Crisp Light & Obsidian Dark Themes
- **Light Mode**: High-definition Slate-400 (#94a3b8) grid lines, distinct Slate-500 checkbox outlines, and vivid week column borders.
- **Dark Mode**: Premium obsidian backdrop (#080c14) with glowing accents.

### ðŸ”’ 100% Offline & Private
- No cloud account, no subscription, and no external tracking.
- All entries are saved locally via browser localStorage.
- Your habit records never leave your machine.

---

## ðŸš€ How to Run

### Method 1: Windows Desktop App (Recommended)
Double-click **HabitTracker.exe** to launch the standalone desktop window with a custom flame streak taskbar icon, borderless styling, and dedicated memory-isolated profile.

### Method 2: In Any Web Browser
Simply open **index.html** in any browser (Chrome, Edge, Firefox, Brave, Safari).

### Method 3: Host on GitHub Pages
1. Push this repository to GitHub.
2. Go to **Settings > Pages**.
3. Set source to the main branch root (/).
4. Access your tracker live on the web from any phone, tablet, or PC!

---

## ðŸ“ Repository Structure

`	ext
HabitTracker/
â”œâ”€â”€ index.html            # Complete single-page application
â”œâ”€â”€ HabitTracker.exe      # Windows desktop launcher executable
â”œâ”€â”€ Launcher.cs           # C# source code for Windows launcher
â”œâ”€â”€ icon256.png           # 256x256 high-resolution application icon
â”œâ”€â”€ flame_streak_v1.ico   # Multi-size Windows icon file
â”œâ”€â”€ app.ico               # Standard icon file
â”œâ”€â”€ run.bat               # Batch script alternative launcher
â”œâ”€â”€ manifest.json         # Web application manifest
â”œâ”€â”€ .gitignore            # Git ignore file
â”œâ”€â”€ LICENSE               # MIT License
â””â”€â”€ README.md             # Project documentation
`

---

## ðŸ› ï¸ Built With

- **HTML5 & CSS3**: Modern glassmorphism, responsive grid, custom SVGs.
- **JavaScript (ES6+)**: Zero-dependency, offline state engine.
- **C# (.NET Framework)**: Native Win32 / Edge WebView wrapper for Windows.

---

## ðŸ“„ License

This project is licensed under the [MIT License](LICENSE).
