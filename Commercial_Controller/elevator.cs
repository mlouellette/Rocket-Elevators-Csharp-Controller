using System.Threading;
using System.Collections.Generic;
using System;

namespace Commercial_Controller
{
    public class Elevator
    {
        public int _ID, _amountOfFloors, _currentFloor; 
        public string status, direction;
        public Door door;
        public bool overweight, obstruction;
        public List<int> floorRequestList;
        public List<int> completedRequestsList;

        public int columnID = 1;
        public int elevatorID = 1;
        public int floorRequestButtonID = 1;
        public int callButtonID = 1;
        public int floor;

        public Elevator(int _ID, string _status, int _amountOfFloors, int _currentFloor)
        {
            this._ID = _ID;
            this.status = _status;
            this._amountOfFloors = _amountOfFloors;
            this._currentFloor = _currentFloor;
            this.door = new Door(_ID, "closed");
            List<int> floorRequestList = new List<int>();
            List<int> completedRequestsList = new List<int>();
            this.direction = "";
            this.overweight = false;
            this.obstruction = false;
            
        }
        // Move elevator to the direction based on what floor we are currently
        public void move()
        {
            while (this.floorRequestList.Count != 0) {
                this.status = "moving";
                int destination = this.floorRequestList[0];
                if (this.direction == "up") {
                    while (_currentFloor < destination) {
                        this._currentFloor++;
                    }
                } else if (this.direction == "down") {
                    while (_currentFloor > destination) {
                        _currentFloor--;
                    }
                }
                this.status = "stopped";
                this.operateDoors();
                this.floorRequestList.RemoveAt(0);
                this.completedRequestsList.Add(destination);
            }
            this.status = "idle";

        }
        
        // Sort list function to pick up other requests mid destination
        public void sortFloorList() {
            if (this.direction == "up") {
                this.floorRequestList.Sort();
            } else {
                this.floorRequestList.Reverse();

            }

        }

        // Door operations
        public void operateDoors() {
            door.status = "opened";
            if (!this.overweight) {
                this.door.status = "closing";
                if (!this.obstruction) {
                    this.door.status = "closed";

            
                } else {
                    this.operateDoors();

                }
            } else {
                while (this.overweight) {
                    Console.WriteLine("OVERWEIGHT!");

                }
                this.operateDoors();
            }

        }

        // Add new requests to the floorRequest list to pick up
        public void addNewRequest(int requestFloor) {
            if (!this.floorRequestList.Contains(requestFloor)) {
                this.floorRequestList.Add(requestFloor);
            }

            if (this._currentFloor < requestFloor) {
                this.direction = "up";
            }

            if (this._currentFloor > requestFloor) {
                this.direction = "down";

            }
         
        }
        
    }
}