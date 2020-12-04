# LennardJonesFluidOnVR

This is the repository for practicing Unity Scripting.
Simple implementation of Lennard-jones fluid running on Oculus Rift.

![LJFluidOnVR_1203_comp](https://user-images.githubusercontent.com/15133454/101038526-f4ad7980-35be-11eb-95d1-ca72b8aaeabf.gif)

## Requirements
- Unity 2019.4.15f

## Project Setup
- Import `Oculus Integration 20.1` from Asset Store.
- Accept the updates.
- Configure settings following https://developer.oculus.com/documentation/unity/unity-conf-settings/ .
- Update Spatializer plugin from Oculus menu.
- Select the Oculus Spatializer Plugin from `Project Setting`>`Audio`.
- Restart Unity.

## Input file format
You can specify each parameter of lennard-jones system, for example, radius of specific particle, size of the simulation box, temperature and so on, by input file based on Toml format.
When you execute simulation from Untiy `play` of Unity application, locate your input file as `(ProjectRoot)/input/input.toml` in Project folder.
When you execute simulation by the binary file in `(ProjectRoot)/bin` folder, locate your input file as `(ProjectRoot)/bin/input/input.toml`.
The detail of file format is like below. This file format is a subset of [Mjolnir](https://github.com/Mjolnir-MD/Mjolnir)'s input file format.
```toml
[[systems]]
attributes.temperature = 300.0
boundary_shape.lower = [-5.0, -5.0, -5.0]
boundary_shape.upper = [ 5.0,  5.0,  5.0]
particles = [
{m =  1.00, pos = [  0.9084463866571024, 2.667970365022592, 0.6623741650618591]}, # particle index 1
{m =  1.00, pos = [-0.39914657537482867, 2.940257103317942, 3.5414659037905025]}, # particle index 2
...
]

[[forcefields]]
[[forcefields.global]]
potential = "LennardJones"
parameters = [
{index =   0, sigma = 0.5, epsilon = 0.05},
{index =   1, sigma = 0.5, epsilon = 0.05},
...
]
```
If you don't specify the rudius of particle, that is set to 0.5.
