# Raport Tehnic - Platformer 2D (Unity)

## 1. Introducere

**Platformer 2D** este o aplicație interactivă de tip joc video, dezvoltată utilizând motorul grafic Unity și limbajul de programare C#. Proiectul își propune să demonstreze competențe în dezvoltarea software orientată pe obiecte, gestionarea fizicii în timp real și designul interfeței cu utilizatorul (UI).

### 1.1 Obiectivul Aplicației
Scopul principal al jucătorului este parcurgerea unei serii de niveluri cu dificultate progresivă. Pentru a finaliza un nivel, utilizatorul trebuie să îndeplinească două obiective strategice:
1.  Neutralizarea tuturor inamicilor prezenți în scenă.
2.  Recuperarea tuturor cheilor distribuite în nivel.

## 2. Arhitectura Software

Sistemul este construit pe o arhitectură modulară, utilizând principiile programării orientate pe obiecte (OOP), în special **Moștenirea** și **Polimorfismul**, pentru a asigura un cod curat și extensibil.

### 2.1 Ierarhia de Clase (Entity System)
Nucleul interacțiunii este clasa abstractă `Entity`, care definește comportamentul fundamental al oricărei ființe din joc.

*   **`Entity` (Clasă de Bază)**
    *   *Responsabilități*: Gestionează componentele Unity esențiale (`Rigidbody2D` pentru fizică, `Animator` pentru animații), detectarea coliziunilor cu solul și sistemul de viață (health).
    *   *Metode Cheie*: `Damage()`, `Die()`, `HandleMovement()`, `HandleCollision()`.
*   **`Player` (Extinde `Entity`)**
    *   *Functionalitate*: Implementează logica specifică controlului uman.
    *   *Input System*: Procesează intrările de la tastatură pentru mișcare orizontală, sărituri și atacuri.
    *   *UI Binding*: Actualizează în timp real HUD-ul (numărul de inimi și bara de energie).
*   **`Enemy` (Extinde `Entity`)**
    *   *Functionalitate*: Definește comportamentul de bază al inamicilor (detectarea jucătorului).
*   **`EnemyPatrol` (Extinde `Enemy`)**
    *   *AI Avansat*: Implementează un automat cu stări finite (FSM - Finite State Machine) simplificat:
        1.  **Patrulare (Patrol)**: Mișcare între două puncte (`leftEdge`, `rightEdge`).
        2.  **Repaus (Idle)**: Pauză la capetele rutei de patrulare.
        3.  **Urmărire (Chase)**: Când jucătorul intră în raza vizuală (`chaseRange`), inamicul își părăsește ruta pentru a ataca.

### 2.2 Design Patterns Utilizate
1.  **Singleton**: Folosit pentru managerii globali care trebuie să existe într-o singură instanță și să fie accesibili de oriunde.
    *   `ObjectiveManager.instance`: Centralizează starea jocului (scor, inamici rămași).
    *   `PauseMenu.instance`: Controlează starea de pauză și panourile UI.
    *   `SceneController.instance`: Gestionează tranzițiile între scene.

## 3. Implementare Tehnică și Gameplay

### 3.1 Sistemul de Luptă și Resurse
*   **Viața (Health)**: Implementată prin `currentHealth`. La fiecare lovitură, aceasta scade. Dacă ajunge la 0, entitatea apelează `Die()`. Vindecarea se face prin `HealthPack`, care apelează metoda `Heal()` din `Player`.
*   **Energia (Stamina)**: Gestionată exclusiv în clasa `Player`.
    *   *Regenerare*: `RegenerateEnergy()` crește valoarea în timp (`Time.deltaTime`).
    *   *Atac Special*: Metoda `HandleSuperAttack()` verifică dacă energia este maximă, declanșează animația și apoi consumă energia în trepte folosind o corutină (`IEnumerator StepDrainEnergy`).

### 3.2 Inteligența Artificială (Enemy AI)
Scriptul `EnemyPatrol.cs` conține logica de detectare.
*   **Detectarea Jucătorului**: Folosește `Physics2D.OverlapCircle` pentru a verifica prezența jucătorului într-o rază definită.
*   **Logica de Urmărire**: Dacă jucătorul este detectat, inamicul calculează direcția (`target.position.x - transform.position.x`) și se deplasează spre el. Se verifică limitele platformei pentru a preveni inamicul să cadă.

### 3.3 Elemente de Mediu
*   **Platforme Mobile**: Scriptul `MovingPlatforms.cs` mută platforma între o serie de puncte.
    *   *Parenting*: Când jucătorul atinge platforma (`OnCollisionEnter2D`), devine "copilul" transform-ului platformei. Aceasta asigură că jucătorul se mișcă odată cu platforma și nu alunecă.

## 4. Interfața Utilizator (User Interface)

Interfața este construită folosind sistemul UI din Unity (Canvas).

### 4.1 Meniul Principal (Main Menu)
*   Include opțiuni pentru **Volum** (Master, Music, SFX) conectate la un `AudioMixer`.
*   Opțiune pentru **Ecran Complet / Fereastră**.
*   Folosește `PlayerPrefs` pentru a salva preferințele utilizatorului între sesiuni.

### 4.2 HUD (Heads-Up Display)
*   **Inimi**: Scriptul `Player` actualizează un array de imagini (`Image[] heartImages`), schimbând sprite-urile între "Full Heart" și "Empty Heart" în funcție de viața curentă.
*   **Energie**: Similar, o bară segmentată care se umple/golește.
*   **Contoare**: `ObjectiveManager` actualizează textul pentru inamici (ex: "3/5") și chei.

### 4.3 Meniuri Contextuale
Jocul gestionează mai multe panouri suprapuse, controlate de `PauseMenu.cs`:
*   **Game Over**: Activată la moartea jucătorului. Oprește timpul (`Time.timeScale = 0`).
*   **Level Complete**: Activată de `ObjectiveManager` când obiectivele sunt atinse.
*   **Pauză**: Activată la apăsarea tastei `ESC`.

## 5. Instrucțiuni de Utilizare (Ghidul Jucătorului)

### Controale
| Acțiune | Tastă / Input | Descriere |
| :--- | :--- | :--- |
| **Mișcare** | `A` / `D` | Deplasare stânga/dreapta |
| **Săritură** | `Space` | Săritură verticală |
| **Atac** | `Click Stânga` | Lovitură simplă cu sabia |
| **Atac Special** | `Shift` + `Click` | Lovitură puternică (Doar cu energie full) |
| **Pauză** | `ESC` | Deschide meniul de pauză |

### Fluxul Jocului
1.  Start din **Main Menu** -> Selectare Nivel.
2.  Explorare nivel, evitare capcane și inamici.
3.  Uciderea inamicilor crește contorul de Kills.
4.  Colectarea cheilor deblochează finalul.
5.  La finalizare, se deblochează următorul nivel (progres salvat prin `PlayerPrefs`).

## 6. Concluzii
Proiectul "Platformer 2D" reprezintă o implementare completă a unui ciclu de joc, integrând mecanici de fizică, inteligență artificială reactivă și un sistem robust de management al stării (UI și date salvate). Structura codului permite adăugarea ușoară de noi inamici, niveluri sau mecanici fără a modifica sistemele de bază.
