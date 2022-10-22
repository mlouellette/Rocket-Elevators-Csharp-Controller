using System;
using System.Collections.Generic;

namespace Commercial_Controller
{
 
    public class Column
    {
        public int _ID, _amountOfFloors, _amountOfElevators;
        public string status;
        public bool _isBasement;
        public List<int> servedFloors;
        public List<Elevator> elevatorsList;
        public List<CallButton> callButtonsList;

        public int columnID = 1;
        public int elevatorID = 1;
        public int floorRequestButtonID = 1;
        public int callButtonID = 1;
        public int floor;
        
        public Column(int _ID, string status, int _amountOfElevators, List<int> _servedFloors, bool _isBasement)
        {
         this._ID = _ID;
         this.status = "online";
         this._amountOfElevators = _amountOfElevators;
         this.callButtonsList = new List<CallButton>();
         this.elevatorsList = new List<Elevator>();
         this.servedFloors = _servedFloors;
         this._isBasement = _isBasement;

         createElevators(_amountOfFloors, _amountOfElevators);
         createCallButtons(_amountOfFloors, _isBasement);
        }
        
        // Function call button for the different scenarios, Elevator going UP or DOWN
        public void createCallButtons(int _amountOfFloors, bool _isBasement) {
            if (this._isBasement) {
                int buttonFloor = -1;
                for (int i = 0; i < _amountOfFloors; i++) {
                    this.callButtonsList.Add(new CallButton(callButtonID, "OFF", buttonFloor, "up"));
                    buttonFloor--;
                    callButtonID++;

                }
                
            } else {
                int buttonFloor = 1;
                for (int i = 0; i < _amountOfFloors; i++) {
                    
                    this.callButtonsList.Add(new CallButton(callButtonID, "OFF", buttonFloor, "down"));
                    buttonFloor++;
                    callButtonID++;

                  }
                }
            }
        
        // Create the elevators necessary for the different scenarios
        public void createElevators(int _amountOfFloors, int _amountOfElevators ) {
            for (int i = 0; i < _amountOfElevators; i++) {
                
                Elevator elevator = new Elevator(this.elevatorID, "idle", _amountOfFloors, 1);
                this.elevatorsList.Add(elevator);
                this.elevatorID++;
            }
        }

        //Simulate when a user press a button on a floor to go back to the first floor
        public Elevator requestElevator(int userPosition, string direction)
        {
            Elevator elevator = this.findElevator(userPosition, direction);
            elevator.addNewRequest(userPosition);
            elevator.move();
            return elevator;
            
        }

        //We use a score system depending on the current elevators state. Since the bestScore and the referenceGap are 
        //higher values than what could be possibly calculated, the first elevator will always become the default bestElevator, 
        //before being compared with to other elevators. If two elevators get the same score, the nearest one is prioritized. Unlike
        //the classic algorithm, the logic isn't exactly the same depending on if the request is done in the lobby or on a floor.
        public Elevator findElevator(int requestedFloor, string requestedDirection) {
            BestElevatorInformation bestElevatorInformation = new BestElevatorInformation();
            // bestElevatorInformation.bestElevator = 3;
            bestElevatorInformation.bestScore = 6;
            bestElevatorInformation.referenceGap = 10000000;

            if (requestedFloor == 1) {
                foreach (Elevator elevator in this.elevatorsList){
                    //The elevator is at the lobby and already has some requests. It is about to leave but has not yet departed
                    if ( 1 == elevator._currentFloor && elevator.status == "stopped") {
                        bestElevatorInformation = this.checkIfElevatorIsBetter(1, elevator, bestElevatorInformation, requestedFloor);
                    //The elevator is at the lobby and has no requests    
                    } else if (1 == elevator._currentFloor && elevator.status == "idle") {
                        bestElevatorInformation = this.checkIfElevatorIsBetter(2, elevator, bestElevatorInformation, requestedFloor);
                    //The elevator is lower than me and is coming up. It means that I'm requesting an elevator to go to a basement, and the elevator is on it's way to me.   
                    } else if (1 > elevator._currentFloor && elevator.direction == "up") {
                        bestElevatorInformation = this.checkIfElevatorIsBetter(3, elevator, bestElevatorInformation, requestedFloor);
                    //The elevator is above me and is coming down. It means that I'm requesting an elevator to go to a floor, and the elevator is on it's way to me  
                    } else if (1 < elevator._currentFloor && elevator.direction == "down") {
                        bestElevatorInformation = this.checkIfElevatorIsBetter(3, elevator, bestElevatorInformation, requestedFloor);
                    //The elevator is not at the first floor, but doesn't have any request    
                    } else if (elevator.status == "idle") {
                        bestElevatorInformation = this.checkIfElevatorIsBetter(4, elevator, bestElevatorInformation, requestedFloor);
                    //The elevator is not available, but still could take the call if nothing better is found    
                    } else {
                        bestElevatorInformation = this.checkIfElevatorIsBetter(5, elevator, bestElevatorInformation, requestedFloor);
                    }
                }
            } else 
                {foreach (Elevator elevator in this.elevatorsList) {
                    //The elevator is at the same level as me, and is about to depart to the first floor
                    if (requestedFloor == elevator._currentFloor && elevator.status == "stopped" && requestedDirection == elevator.direction) {
                        bestElevatorInformation = this.checkIfElevatorIsBetter(1, elevator, bestElevatorInformation, requestedFloor);
                    //The elevator is lower than me and is going up. I'm on a basement, and the elevator can pick me up on it's way    
                    } else if (requestedFloor > elevator._currentFloor && elevator.direction == "up" && requestedDirection == "up") {
                        bestElevatorInformation = this.checkIfElevatorIsBetter(2, elevator, bestElevatorInformation, requestedFloor);
                    //The elevator is higher than me and is going down. I'm on a floor, and the elevator can pick me up on it's way    
                    } else if (requestedFloor < elevator._currentFloor && elevator.direction == "down" && requestedDirection == "down") {
                        bestElevatorInformation = this.checkIfElevatorIsBetter(2, elevator, bestElevatorInformation, requestedFloor);
                    //The elevator is idle and has no requests  
                    } else if (elevator.status == "idle") {
                        bestElevatorInformation = this.checkIfElevatorIsBetter(4, elevator, bestElevatorInformation, requestedFloor);
                    //The elevator is not available, but still could take the call if nothing better is found    
                    } else {
                        bestElevatorInformation = this.checkIfElevatorIsBetter(5, elevator, bestElevatorInformation, requestedFloor);
                    }
                }
            }
            return bestElevatorInformation.bestElevator;
        }
        
        // Select the closest/best elevator to do the scenario
        public BestElevatorInformation checkIfElevatorIsBetter(int scoreToCheck, Elevator newElevator, BestElevatorInformation bestElevatorInformation, int floor) {
            if (scoreToCheck < bestElevatorInformation.bestScore) {
                bestElevatorInformation.bestScore = scoreToCheck;
                bestElevatorInformation.bestElevator = newElevator;
                bestElevatorInformation.referenceGap = Math.Abs(newElevator._currentFloor - floor);
            } else if (bestElevatorInformation.bestScore == scoreToCheck) {
                int gap = Math.Abs(newElevator._currentFloor - floor);
                if (bestElevatorInformation.referenceGap > gap) {
                    bestElevatorInformation.bestElevator = newElevator;
                    bestElevatorInformation.referenceGap = gap;

                }

            }
            return bestElevatorInformation;

        }

    }

}





    

    
