using System.Collections;
using System.Collections.Generic;
using System;
//using UnityEngine;


namespace SullysToolkit
{
    //Enums & Interfaces

 

    /*
    public interface ICard
    {
        CardDeck GetOriginDeck();

        void SetOriginDeck(CardDeck newDeck);

        void ClearOriginDeck();

        string GetCardName();
    }
    */


    /*
    public class CardDeck
    {
        //Declarations
        private List<Card> _decklist;
        private List<Card> _currentCardsInDeck;




        //Constructors
        public CardDeck()
        {
            _decklist = new List<Card>();
            _currentCardsInDeck = new List<Card>();
        }

        public CardDeck(List<Card> newDecklist)
        {
            foreach (Card card in newDecklist)
                AddNewCardToDecklist(card);
        }



        //Getters, Setters, & Commands
        public int GetCurrentCardCount()
        {
            return _currentCardsInDeck.Count;
        }

        public int GetDecklistCount()
        {
            return _decklist.Count;
        }

        public bool IsCardInDecklist(Card card)
        {
            return _decklist.Contains(card);
        }

        public bool IsCardCurrentlyInDeck(Card card)
        {
            return _currentCardsInDeck.Contains(card);
        }

        public void AddNewCardToDecklist(Card newCard)
        {
            if (!IsCardInDecklist(newCard))
            {
                _decklist.Add(newCard);
                _currentCardsInDeck.Insert(0,newCard);
                newCard.SetOriginDeck(this);
            }
                
        }

        public void DeleteCardFromDecklist(Card card)
        {
            if (IsCardInDecklist(card))
            {
                _currentCardsInDeck.Remove(card);
                _decklist.Remove(card);
                card.ClearOriginDeck();
            }
        }

        public void ReturnCardToDeck(Card card, int returnLocation)
        {
            if (IsCardInDecklist(card) && !IsCardCurrentlyInDeck(card))
            {
                returnLocation = Mathf.Clamp(returnLocation, 0, _currentCardsInDeck.Count);
                _currentCardsInDeck.Insert(returnLocation, card);
            }
                
        }

        public void PullSpecificCardFromDeck(Card card)
        {
            if (IsCardCurrentlyInDeck(card))
                _currentCardsInDeck.Remove(card);
        }

        public Card PullCardFromLocationFromTop(int topOffset = 0)
        {
            if (GetCurrentCardCount() > 0)
            {
                topOffset = Mathf.Clamp(topOffset, 0, _currentCardsInDeck.Count - 1);
                int drawIndex = _currentCardsInDeck.Count - topOffset;

                Card drawnCard = _currentCardsInDeck[drawIndex];
                _currentCardsInDeck.RemoveAt(drawIndex);
                return drawnCard;
            }
            else return null;


        }

        public void ShuffleDeck()
        {
            List<Card> shuffledList = new List<Card>();

            int shuffledCardCount = 0;
            int heldCardCount = _currentCardsInDeck.Count;
            while (shuffledCardCount < heldCardCount)
            {
                Card randomCard = _currentCardsInDeck[Random.Range(0, _currentCardsInDeck.Count)];
                _currentCardsInDeck.Remove(randomCard);
                shuffledList.Add(randomCard);
                shuffledCardCount++;
            }

            _currentCardsInDeck = shuffledList;
        }

        public Card InspectTopCardOfDeck(int topOffset = 0)
        {
            topOffset = Mathf.Clamp(topOffset, 0, _currentCardsInDeck.Count);
            return _currentCardsInDeck[_currentCardsInDeck.Count - topOffset];
        }


        //Debugging
        public void LogDecklist()
        {
            int indexCounter = 0;
            Debug.Log("==== Decklist ====");
            foreach(Card card in _decklist)
            {
                Debug.Log($"Index: {indexCounter}, Card: {card}");
                indexCounter++;
            }
            Debug.Log("==== End of Decklist ====");
        }

        public void LogCurrentDeckContents()
        {
            int topOffset = 0;
            int cardsInDeck = _currentCardsInDeck.Count;

            Debug.Log("==== Current Deck Contents ====");
            Debug.Log("--- Contents Listed from Top of deck to Bottom ---");
            for (int backwardsSteppingIndexer = _currentCardsInDeck.Count - 1; backwardsSteppingIndexer >= 0; backwardsSteppingIndexer--)
            {
                Debug.Log($"Position from Top: {topOffset}, Card: {_currentCardsInDeck[backwardsSteppingIndexer]}");
                topOffset++;
            }
            Debug.Log("==== End of Deck ====");
        }
    }
    

    public class Card : ICard
    {
        //Declarations
        private string _cardName;
        private CardDeck _originDeck;


        //Constructors
        public Card(string name, CardDeck originDeck = null)
        {
            _cardName = name;

            if (originDeck != null)
            {
                originDeck.AddNewCardToDecklist(this);
                _originDeck = originDeck;
            }
        }


        //Interface Utils
        public CardDeck GetOriginDeck()
        {
            return _originDeck;
        }

        public void SetOriginDeck(CardDeck newDeck)
        {
            if (newDeck != null)
                _originDeck = newDeck;
        }

        public void ClearOriginDeck()
        {
            _originDeck = null;
        }

        public string GetCardName()
        {
            return _cardName;
        }

    }
    */
}

