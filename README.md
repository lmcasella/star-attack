# 🚀 Star Attack

[![Jugar en itch.io](https://img.shields.io/badge/Jugar_en-itch.io-FA5C5C?style=for-the-badge&logo=itch.io)](https://kezgan.itch.io/star-attack)

Un videojuego *shoot 'em up* en 2D de temática espacial, desarrollado en **Unity** como proyecto universitario. El jugador debe manejar una nave, esquivar obstáculos y sobrevivir a oleadas sucesivas de enemigos que varían en dificultad, gestionando sus vidas y maximizando su puntaje.

## 🎮 Características Principales

* **Sistema de Oleadas:** Sobreviví a 4 fases de combate intensas, cada una con su propio temporizador. La dificultad progresa aumentando la cantidad de enemigos simultáneos en pantalla.
* **Combate y Movimiento:** Movimiento horizontal y vertical. El jugador dispara en la dirección en la que está mirando para destruir las naves enemigas.
* **Comportamiento Enemigo:** Los enemigos se generan en posiciones aleatorias verticalmente y se desplazan horizontalmente. Requieren múltiples impactos para ser destruidos.
* **Sistema de Castigo y Recompensa:** El puntaje otorgado por enemigo eliminado escala con cada oleada. Perdés una vida si una nave enemiga te impacta o si logra cruzar todo el escenario y escapar.
* **Condiciones de Victoria/Derrota:** Pantallas de fin de juego dinámicas que muestran el puntaje final y permiten reiniciar la partida.

## ⚙️ Arquitectura y Patrones de Diseño

El código fue escrito priorizando un diseño limpio, modular y configurado mediante el Inspector de Unity para facilitar el diseño de niveles sin alterar los *scripts*:

* **GameManager Centralizado:** Se encarga del flujo global de la partida, controlando el cambio de estados, la cuenta regresiva, el puntaje, las vidas y la transición entre menús.
* **Patrón Strategy (Scriptable Objects):** El sistema de armamento está desacoplado utilizando `ScriptableObjects` (`SingleShotPattern`, `BurstFirePattern`). Esto permite crear y asignar nuevos tipos de disparo desde el editor de manera instantánea y escalable.
* **Físicas y Colisiones Optimizadas:** Detección de impactos y limpieza de memoria automática. Los proyectiles y enemigos se destruyen al colisionar o al salir de los límites de la cámara.
* **Prefabs:** Instanciación dinámica de naves y proyectiles, asegurando consistencia y control de rendimiento.

## 🕹️ Controles Básicos

* **Movimiento:** `W`, `A`, `S`, `D` / Flechas direccionales
* **Disparar:** `Espacio` / `Clic Izquierdo`
* **Pausa:** `Escape`

## 🚀 Cómo probar el proyecto en Unity

1. Cloná este repositorio: `git clone https://github.com/lmcasella/star-attack.git`
2. Abrí el proyecto utilizando **Unity Hub**.
3. Andá a la carpeta `Assets/Scenes` y abrí la escena `MainMenuScene`.
4. Play en el editor.
