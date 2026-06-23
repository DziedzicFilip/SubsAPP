# 🖥️ Struktura plików — Frontend Web

## Technologie

| Narzędzie | Wersja | Opis |
|-----------|--------|------|
| React | 19.2 | Biblioteka UI |
| TypeScript | ~5.9 | Typowanie statyczne |
| Vite | 7.x | Bundler i dev server |
| ESLint | 9.x | Linting kodu |

---

## 📁 Drzewo katalogów

```
FrontendWeb/
├── index.html                      # Punkt wejścia HTML
├── vite.config.ts                  # Konfiguracja Vite
├── tsconfig.json                   # Konfiguracja TypeScript (main)
├── tsconfig.app.json               # Konfiguracja TS dla aplikacji
├── tsconfig.node.json              # Konfiguracja TS dla Node (vite config)
├── eslint.config.js                # Konfiguracja ESLint
├── package.json                    # Zależności i skrypty npm
├── .env                            # Zmienne środowiskowe (VITE_API_URL)
├── .gitignore                      # Pliki ignorowane przez git
│
├── public/                         # Zasoby statyczne (serwowane as-is)
│
└── src/                            # Kod źródłowy aplikacji
    ├── main.tsx                    # Punkt wejścia React (renderuje <App/>)
    ├── App.tsx                     # Główny komponent aplikacji
    ├── App.css                     # Style komponentu App
    ├── index.css                   # Globalne style CSS
    │
    ├── Models/                     # Typy TypeScript (modele danych)
    │   └── WeatherForcastModel.ts  # Model danych pogodowych (przykład)
    │
    ├── services/                   # Warstwa komunikacji z API
    │   └── WeatherForecastService.ts   # Serwis HTTP (przykład fetch)
    │
    ├── hooks/                      # Custom React Hooks
    │   └── useWetherForecastHook.tsx   # Hook z logiką pobierania danych
    │
    └── assets/                     # Zasoby (obrazy, SVG, fonty)
```

---

## 🏗️ Architektura komponentów

```
main.tsx
  └── <App/>
        └── hooks/useWeatherForecastHook
                └── services/WeatherForecastService
                        └── fetch → Backend API
```

### Wzorzec warstw

| Warstwa | Folder | Odpowiedzialność |
|---------|--------|-----------------|
| **View** | `src/` (`App.tsx`, etc.) | Renderowanie UI, obsługa zdarzeń |
| **Logic** | `hooks/` | Stan, efekty, logika komponentu |
| **Data** | `services/` | Komunikacja z API (fetch/axios) |
| **Types** | `Models/` | Interfejsy i typy TypeScript |

---

## ⚙️ Skrypty npm

```bash
npm run dev        # Uruchamia dev server (http://localhost:5173)
npm run build      # Buduje bundle produkcyjny (dist/)
npm run lint       # Sprawdza kod ESLintem
npm run preview    # Podgląd builda produkcyjnego
```

---

## 🔌 Zmienna środowiskowa

```env
# .env
VITE_API_URL=https://localhost:5001
```

Dostępna w kodzie jako:
```ts
const apiUrl = import.meta.env.VITE_API_URL;
```

---

## 📌 Planowane rozszerzenia (TODO)

| Funkcja | Opis |
|---------|------|
| `src/pages/` | Podział na strony (Login, Dashboard, Schedule) |
| `src/components/` | Współdzielone komponenty UI |
| `src/context/` | Context API (AuthContext, UserContext) |
| `src/router/` | React Router — nawigacja |
| `src/services/authService.ts` | Logowanie, przechowywanie tokenu JWT |
| `src/services/groupsService.ts` | Operacje CRUD na grupach |
