# Rocket-Elevators-Csharp-Controller
 Usage example

- dotnet test

- dotnet test -v n

## Description
This program is a controller for an elevator installation which takes the input from outside the elevator to call the best possible elevator and pick up the person on the right floor. Then once inside it, they can select wichever floor input they want to go to and takes them to the destination. The code is able to run through multiple scenarios with different floors, different directions and different stops.

### Dependencies

As long as you have **.NET 6.0** installed on your computer, nothing more needs to be installed:

The code to run the scenarios is included in the Commercial_Controller folder, and can be executed there with:

- dotnet run <SCENARIO-NUMBER>`

To launch the tests, make sure to be at the root of the repository and run:

- dotnet test

You can also get more details about each test by adding the `-v n` flag: 

- dotnet test -v n 
