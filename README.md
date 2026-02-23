# AR Survival Shooter

## Overview

**AR Survival Shooter** is an Augmented Reality (AR) game developed in **C#** using Unity. The project provides an immersive experience where players engage in survival-based shooting gameplay within an augmented reality environment.

## Project Details

- **Language**: C#
- **Platform**: Augmented Reality (AR)
- **Repository**: [DUSHIME1212/AR-Survival-Shooter](https://github.com/DUSHIME1212/AR-Survival-Shooter)
- **Status**: Active Development

## Features

### AR Implementation
The AR component of this project enables:
- **Real-world environment integration**: Blend virtual game elements seamlessly with the player's physical surroundings
- **Spatial awareness**: The game understands and responds to the player's physical space and positioning
- **Interactive gameplay**: Players can move around their environment while engaging with virtual enemies and obstacles

### Core Gameplay
- **Survival mechanics**: Players must survive waves of enemies while managing resources
- **Shooting mechanics**: Action-oriented gameplay with interactive firing systems
- **Immersive AR experience**: Play in your own environment with AR-rendered enemies and effects

## How It Works

### AR System Architecture

1. **AR Foundation Integration**
   - Utilizes AR frameworks for camera tracking and plane detection
   - Establishes real-world object placement and physics

2. **Game Objects & Enemies**
   - Virtual enemies spawn in the AR environment
   - Players aim and shoot using device controls or gestures
   - Real-time collision detection between shots and targets

3. **Input System**
   - Device motion and camera input for aiming
   - Touch controls for shooting mechanics
   - Gesture recognition for player movement and actions

4. **AR Graphics Rendering**
   - 3D model rendering in augmented space
   - Visual effects for shots, explosions, and enemy interactions
   - Smooth animation transitions between game states

## Technical Stack

- **Engine**: Unity
- **Language**: C#
- **AR Technology**: AR Foundation / ARKit / ARCore
- **Physics**: Unity Physics Engine
- **UI**: Unity UI Framework

## Installation

### Prerequisites
- Unity 2020 LTS or higher
- AR-capable mobile device (iOS with ARKit or Android with ARCore)
- Mobile development SDK installed

### Setup Steps

1. Clone the repository:
   ```bash
   git clone https://github.com/DUSHIME1212/AR-Survival-Shooter.git