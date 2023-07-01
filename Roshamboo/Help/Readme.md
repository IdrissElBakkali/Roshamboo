# RoShamBoo Game

This REST API allows the user to play the famous game 'Rock, Paper, Scissors' against the AI.

## Game Mechanic

The goal of the game is to win more rounds than the opponent (the computer).

The user initiates a game against the AI by specifying a number of Rounds to play, then at each round, he submits a Shape to play: 'Rock', 'Paper, or 'Scissors'.

For each round, the game server compares the user shape with the AI shape using the following logic : 'Rock' < 'Paper < 'Scissors' < 'Rock'.

## Usage

To start a game, the user needs to send a POST request to the following URL : https://localhost:port/roshamboo/CreateGame

The user needs to specify in the request form of the POST request:
	- The number of rounds, as an integer, between 5 and 10.
	
Example of a request:
curl -X 'POST' \
  'https://localhost:44375/roshamboo/CreateGame' \
  -H 'accept: text/plain' \
  -H 'Content-Type: multipart/form-data' \
  -F 'Rounds=5'
  
This will generate a response from the server with the new game Id.

For example :
Game "edc10196-8869-4e99-a844-1582fa42064d" started, user score: 0, computer score: 0

For each round, the user will send another POST request to the following URL : https://localhost:port/roshamboo/PlayGame

The user needs to specify in the request form of the POST request:
	- The Game Id, as a string
	- Teh chosen Shape, as a string

Example of a request:
curl -X 'POST' \
  'https://localhost:44375/roshamboo/PlayGame' \
  -H 'accept: text/plain' \
  -H 'Content-Type: multipart/form-data' \
  -F 'Id=edc10196-8869-4e99-a844-1582fa42064d' \
  -F 'Shape=Paper'

This will generate the current state of the game played and will be updated at each round. 

Here is an example of a response after a player plays a hand:
curl -X 'POST' \
  'https://localhost:44375/roshamboo/PlayGame' \
  -H 'accept: text/plain' \
  -H 'Content-Type: multipart/form-data' \
  -F 'Id=edc10196-8869-4e99-a844-1582fa42064d' \
  -F 'Shape=Paper'