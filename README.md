# Rocket-Elevators-Csharp-Controller

## Usage Example

Run `dotnet test` to execute the tests.
Run `dotnet test -v` n for more detailed test output.

# Description
This program serves as a controller for an elevator installation. It receives input from outside the elevator to determine the best possible elevator to call and pick up the person on the correct floor. Once inside, the person can select their desired floor, and the elevator will transport them to their destination. The code is capable of handling multiple scenarios with different floors, directions, and stops.

# Dependencies
To run this program, you need to have .NET 6.0 installed on your computer. No additional dependencies are required.

- To execute the scenarios, navigate to the Commercial_Controller folder and run:
`dotnet run SCENARIO-NUMBER`

- To run the tests, ensure that you are at the root of the repository and execute:
`dotnet test`

- For more detailed test output, use the -v n flag:
`dotnet test -v n`
