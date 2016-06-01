contract Task {
  /**
   * It is helpful to think of
   * smart contracts as state machine.
   * In this example:
   * State 1: Deploy new smart task contract
   * State 2: Set task name and reward
   * State 3: Task Completed
   * State 4: Task Deleted
   */

  address owner;
  address completedBy;

  uint taskReward;

  string stateMessage;
  uint stateInt;

  string taskName;
  string taskDescription;

  function Task() {
    owner = msg.sender;

    stateMessage = "Task uploaded";
    stateInt = 1;
  }

  /**
   * Set the details specific to this task
   */
  function setUpTaskDetails(uint reward, string name, string description) returns (string){
    if(reward >= ((this.balance + msg.value) / 1000000000000000000))
    {
	  msg.sender.send(msg.value);
      return "Not enough ether sent as reward";
    }
    taskReward = reward;
    stateMessage = "Task details set";
    taskName = name;
    taskDescription = description;
    stateInt = 2;
    return stateMessage;
  }

  /**
   * Complete the task contract
   */
  function completeTask() returns (string){
    completedBy = msg.sender;
    completedBy.send(taskReward * 1 ether);
    stateInt = 3;
    stateMessage = "Task successfully completed";

    return stateMessage;
  }
  
  function deleteTask() returns (string){
        owner.send(this.balance);
        stateInt = 4;
        stateMessage = "Deleted Task";
        return stateMessage;
  }

}