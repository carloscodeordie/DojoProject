0. Introduction

Easy VHS is a set of camera effects emulating effects and visual artifacts of CRT television and VHS players.

They look great in retro-style games and can be used whenever you want to add some extra authenticity and oldschool aesthetic to your creations.

The pack includes:

VHS Blur:
- Separate blur on different channels creating a unique look
- Emulates the look of VHS recordings
- The amount of blur can be adjusted with sliders

NTSC:
- Effect emulating the look of NTSC signal
- Y, I, Q components of the image can be adjusted with sliders

TV Distortion
- Includes moving noise, fisheye lens effect, vertical/horizontal distortion, scanlines, vignetting, horizontal stripes
- The strength of aforementioned effects can be adjusted separately allowing for many unique combinations

CRT
- Simulates the look of a CRT screen which divides individual pixels into RGB components.

These three components are completely separate and can be linked in different order producing different effects. Experiment to see what effects you like most.

1. How to use

You will need image effects from the Standard Assets package in order for the image effects to work.

The simplest way to use Easy VHS is to add the included CameraPrefab to the scene, and set it as main camera. It has all three effects attached to it. Then you can adjust the effects as you see fit using the sliders.

You can also add the scripts to your camera manually. To find out how to configure the scripts, take a look at CameraPrefab. 

2. Effects

VHS Blur replicates the look of old, deteriorated VHS recordings. Blur amount controls how much to blur the image overall, while Channel Dif controls the difference in blurring separate channels, introducing an effect resembling chromatic aberration, but softer.

NTSC emulates signal degeneration when using the NTSC standard. The image is converted to YIQ components and then decoded back. The components can be controlled with sliders to create many unique effects.

TV Distortion combines several effects into one, replicating defects of old CRT television sets. Distortion Strength controls the strength of the vertical and horizontal distortion of the whole image. Fisheye Strength applies a fisheye-lens-like deformation to the image. Stripes strength controls visibility of noisy horizontal stripes. Noise strength adds a filter of static across the whole image. Vignette strength darkens the corners of the screen. VHS scanlines causes horizontal bars of distortion to crawl across the screen up and down. 
This effect can have a custom noise texture applied, an example texture that looks good is included.

CRT simulates a CRT display by dividing the image into very small red, green, and blue bars, separated by tiny empty spaces. You can regulate the size of those bars with a slider.
Likely, you won't use the effect with your game's main camera, but it could look interesting when you apply it to an in-game television screen, adding some extra authenticity.
Due to the individual bars being separated, this effect dims the brightness of the image. This is partially mitigated in the shader by multiplying the final colour by 1.2.

3. Contact us

If you have any further questions please see our Unity Store page or contact us directly at greygolem@openmailbox.org.