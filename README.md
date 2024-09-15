
[![Watch the video](https://img.youtube.com/vi/ksaUojnINoo/maxresdefault.jpg)](https://www.youtube.com/watch?v=ksaUojnINoo&t=20s)


# 🐍 Snake 2D Game

A modern take on the classic Snake game with exciting features like powerups and a two-player mode!

## 🕹️ Game Features

### 🎮 Game Modes:
- **Single Player Mode**: Control one snake using arrow keys.
- **Two Player Mode**:
    - Player 1: Arrow keys
    - Player 2: WASD keys

### 🍏 Food Types:
- **Mass Gainer**: Increases the snake's length by a customizable amount (default: 3 units) and adds 3 points to the score.
- **Mass Burner**: Reduces snake length by 1 unit which is customizable and also decreases score by 1

### ⚡ Powerups:
- **Shield**: Grants 10 seconds of immunity from self-collision.
- **Score Boost**: Doubles score additions or deductions for 10 seconds.
- **Speed Boost**: Increases snake movement speed for 10 seconds.
- Powerups spawn randomly and disappear if not collected in time.

### 🎯 Additional Features:
- **Score Calculation**: Keeps track of and displays the score.
- **Save System**: Utilizes `PlayerPrefs` to save and show the high score.
- **Camera Shake**: Activated when the snake collects a Mass Burner or any powerup.
- **UI Highlighting**: Visual indicators for powerup activation/deactivation.

---

## 🚀 How to Play

1. **Start the Game**:  
   - For single-player mode, use the arrow keys to control the snake.
   - In two-player mode, control Player 1 with arrow keys and Player 2 with WASD keys.
   
2. **Collect Food and Powerups**:  
   - Mass Gainer increases length and score, while Mass Burner reduces length. Use powerups strategically.

3. **Avoid Collisions**:  
   - Don't collide with yourself unless you're shielded.

---

## 🖼️ Screenshots

### Title Screen Mode


### Single Player Mode with shield active


### Two Player Mode


### Powerups Active


---

## 💾 Save System

Your high score is automatically saved using `PlayerPrefs`, ensuring that your progress is preserved between sessions.

---

## 🔧 Installation and Setup

1. Clone this repository to your local machine:
   ```bash
   git clone https://github.com/your-username/snake-game.git
   cd snake-game



