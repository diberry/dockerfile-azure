'use strict';

const Personalizer = require('@azure/cognitiveservices-personalizer');
const CognitiveServicesCredentials = require('@azure/ms-rest-azure-js').CognitiveServicesCredentials;
const readline = require('readline-sync');

const uuidv1 = require('uuid');

async function main() {

  // The key specific to your personalization service instance; e.g. "0123456789abcdef0123456789ABCDEF"
  let serviceKey = process.env["PERSONALIZER_KEY"];


  // The endpoint specific to your personalization service instance; e.g. https://westus2.api.cognitive.microsoft.com
  let baseUri = process.env["PERSONALIZER_ENDPOINT"];

  let credentials = new CognitiveServicesCredentials(serviceKey);

  // Initialize Personalization client.
  let personalizerClient = new Personalizer.PersonalizerClient(credentials, baseUri);

  let runLoop = true;

  do {

    // this isn't correct - it looks like it is off of mappers instead of models
    /*
    let rankRequest = new Personalizer.PersonalizerModels.RankRequest();
     */

    let rankRequest = {}

    // Generate an ID to associate with the request.
    rankRequest.eventId = uuidv1();

    // Get context information from the user.
    rankRequest.contextFeatures = getContextFeaturesList();

    // Get the actions list to choose from personalization with their features.
    rankRequest.actions = getActionsList();

    // Exclude an action for personalization ranking. This action will be held at its current position.
    rankRequest.excludedActions = getExcludedActionsList();

    rankRequest.deferActivation = false;

    // Rank the actions
    let rankResponse = await personalizerClient.rank(rankRequest);

    console.log("\nPersonalization service thinks you would like to have:\n")
    console.log(rankResponse.rewardActionId);

    // Display top choice to user, user agrees or disagrees with top choice
    let reward = getReward();

    console.log("\nPersonalization service ranked the actions with the probabilities as below:\n");
    for (var i = 0; i < rankResponse.ranking.length; i++) {
      console.log(JSON.stringify(rankResponse.ranking[i]) + "\n");
    }

    // Send the reward for the action based on user response.

    // this isn't correct - it looks like it is off of mappers instead of models
    /*
    let rewardRequest = Personalizer.PersonalizerModels.RewardRequest = {
      value: reward
    };
    */

    let rewardRequest = {
      value: reward
    }

    await personalizerClient.events.reward(rankRequest.eventId, rewardRequest);

    runLoop = continueLoop();

  } while (runLoop);
}

function continueLoop() {
  var answer = readline.question("\nPress q to break, any other key to continue.\n")
  if (answer.toLowerCase() === 'q') {
    return false;
  }
  return true;
}

function getReward() {
  var answer = readline.question("\nIs this correct (y/n)\n");
  if (answer.toLowerCase() === 'y') {
    console.log("\nGreat| Enjoy your food.");
    return 1;
  }
  console.log("\nYou didn't like the recommended food choice.");
  return 0;
}

function getContextFeaturesList() {
  var timeOfDayFeatures = ['morning', 'afternoon', 'evening', 'night'];
  var tasteFeatures = ['salty', 'sweet'];

  var answer = readline.question("\nWhat time of day is it (enter number)? 1. morning 2. afternoon 3. evening 4. night\n");
  var selection = parseInt(answer);
  var timeOfDay = selection >= 1 && selection <= 4 ? timeOfDayFeatures[selection - 1] : timeOfDayFeatures[0];

  answer = readline.question("\nWhat type of food would you prefer (enter number)? 1. salty 2. sweet\n");
  selection = parseInt(answer);
  var taste = selection >= 1 && selection <= 2 ? tasteFeatures[selection - 1] : tasteFeatures[0];

  console.log("Selected features:\n");
  console.log("Time of day: " + timeOfDay + "\n");
  console.log("Taste: " + taste + "\n");

  return [
    {
      "time": timeOfDay
    },
    {
      "taste": taste
    }
  ];
}

function getExcludedActionsList() {
  return [
    "juice"
  ];
}

function getActionsList() {
  return [
    {
      "id": "pasta",
      "features": [
        {
          "taste": "salty",
          "spiceLevel": "medium"
        },
        {
          "nutritionLevel": 5,
          "cuisine": "italian"
        }
      ]
    },
    {
      "id": "ice cream",
      "features": [
        {
          "taste": "sweet",
          "spiceLevel": "none"
        },
        {
          "nutritionalLevel": 2
        }
      ]
    },
    {
      "id": "juice",
      "features": [
        {
          "taste": "sweet",
          "spiceLevel": "none"
        },
        {
          "nutritionLevel": 5
        },
        {
          "drink": true
        }
      ]
    },
    {
      "id": "salad",
      "features": [
        {
          "taste": "salty",
          "spiceLevel": "low"
        },
        {
          "nutritionLevel": 8
        }
      ]
    }
  ];
}

var program = main()
.then(result => console.log("done"))
.catch(err=> console.log(err));
