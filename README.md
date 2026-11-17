# Editor 3D Android

App Android con .NET MAUI 10, pantalla HTML/CSS/JavaScript local y lógica nativa C#. La web corre dentro de `HybridWebView` (sin URL remota), Three.js se sirve desde los archivos incluidos en la app y el puente híbrido maneja guardado, importación y compartir escenas JSON.

Incluye visor 3D táctil, órbita/zoom, cuadrícula, primitivas cubo/esfera, jerarquía, material/color, posición y rotación numérica, escala, borrar y deshacer básico.

## APK de desarrollo
Compila desde GitHub Actions con el workflow manual `Build Android APK`; baja `Editor3D-Android-APK` desde los artefactos de la ejecución. CI exitoso prueba la compilación, no la interacción real en un teléfono.

## Alcance
Primera versión corregida, aún no un editor profesional completo. No ofrece todavía importación GLB, gizmos de arrastre, animación, rigging/IK ni render offline. No afirmar compatibilidad con cada dispositivo hasta probarlos físicamente.
