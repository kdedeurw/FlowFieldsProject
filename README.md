# FlowFieldsProject
Research Project about FlowField pathfinding for Gameplay Programming Exam

Flow Fields pathfinding algorithm:
A flow field basically is a vector grid, poiting towards an endgoal cell.
Every cell therefore has a vector direction pointing at the neighbouring cell
which is closest relative to the given endgoal cell.
Taking costs, impassable cells and relative distance into account.
The resulting vector field is one that almost visualises the flow of a fluid.
The name "Flow Field" actually derives from what most can call a "Fluid Field",
which also basically boils down to a vector grid.

Design/implementation:
I chose the Unity game engine to create this project,
simply because it fit the scope fairly well and Unity gives you access to
quick and easy rendering abilities, which proved useful.
At first I had some minor issues with the grid creation algorithm
since I tried figuring it out on my own from the pseudo code I've read.
I then reverted to doing some more research and watching a couple of videos
before grasping the topic optimally and being able to create some interesting (and mostly correct) results.

Result:
The resulting work perfectly shows the workings of a flow field
and the units roaming around it behave properly minus a few irrelevant steering bugs.

Conclusion/Future work:
This project has taken me around 3-4 days and obviously taught me a bunch about flow fields.
I am pretty eager to use this soon in some future project, if not for school.
I still feel like there is so much more to optimize and implement, 
together with making a hybrid pathfinding algorithm that can smooth and optimize steering movements.
