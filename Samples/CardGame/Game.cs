using LanguageExt;
using static LanguageExt.Prelude;

namespace CardGame;

/// <summary>
/// Pontoon / Vingt-Un / 21
/// </summary>
public partial class Game
{
    /// <summary>
    /// Play the game!
    /// </summary>
    public static Game<Unit> play =>
        from _0 in Display.askPlayerNames
        from _1 in enterPlayerNames
        from _2 in Display.introduction
        from _3 in Deck.shuffle
        from _4 in playHands
        select unit;

    /// <summary>
    /// Ask the users to enter their names until `enterPlayerName` returns `false`
    /// </summary>
    static Game<Unit> enterPlayerNames =>
        when(enterPlayerName, lazy(() => enterPlayerNames)).As();

    /// <summary>
    /// Wait for the user to enter the name of a player, then add them to the game 
    /// </summary>
    static Game<bool> enterPlayerName =>
        from name in Console.readLine
        from _    in when(notEmpty(name), addPlayer(name))
        select notEmpty(name);

    /// <summary>
    /// Play many hands until the players decide to quit
    /// </summary>
    static Game<Unit> playHands =>
        from _0  in initPlayers
        from _1  in playHand
        from _2  in Display.askPlayAgain
        from key in Console.readKey
        from _3  in when(key.Key == ConsoleKey.Y, playHands)
        select unit;

    /// <summary>
    /// Play a single hand
    /// </summary>
    static Game<Unit> playHand =>
        from _1     in dealHands
        from _2     in playRound
        from _3     in gameOver
        from remain in Deck.cardsRemaining
        from _4     in Display.cardsRemaining(remain)
        select unit;

    /// <summary>
    /// Deal the initial cards to the players
    /// </summary>
    static Game<Unit> dealHands =>
        Players.with(Game.players, dealHand);

    /// <summary>
    /// Deal the two initial cards to a player
    /// </summary>
    static Game<Unit> dealHand =>
        from _1     in dealCard
        from _2     in dealCard
        from player in Player.current
        from state  in Player.state
        from _3     in Display.playerState(player, state)
        select unit;

    /// <summary>
    /// Deal a single card
    /// </summary>
    static Game<Unit> dealCard =>
        from card in Deck.deal
        from _    in Player.addCard(card)
        select unit;
    
    /// <summary>
    /// For each active player check if they want to stick or twist
    /// Keep looping until the game ends
    /// </summary>
    static Game<Unit> playRound =>
        when(isGameActive,
             from _ in Players.with(activePlayers, stickOrTwist) 
             from r in playRound
             select r)
            .As();

    /// <summary>
    /// Ask the player if they want to stick or twist, then follow their instruction
    /// </summary>
    static Game<Unit> stickOrTwist =>
        when(isGameActive,
             from player in Player.current
             from _0     in Display.askStickOrTwist(player)
             from _1     in Player.showCards
             from key    in Console.readKey
             from _2     in key.Key switch
                            {
                                ConsoleKey.S => Player.stick,
                                ConsoleKey.T => twist,
                                _            => stickOrTwistBerate
                            }
             select unit)
           .As();

    /// <summary>
    /// Player wants to twist
    /// </summary>
    static Game<Unit> twist =>
        from card in Deck.deal
        from _1   in Player.addCard(card)
        from _2   in Display.showCard(card)
        from _3   in when(Player.isBust, Display.bust)
        select unit;

    /// <summary>
    /// Berate the user for not following instructions!
    /// </summary>
    static Game<Unit> stickOrTwistBerate =>
        from _1 in Display.stickOrTwistBerate
        from _2 in stickOrTwist
        select unit;

    /// <summary>
    /// Show the game over summary
    /// </summary>
    static Game<Unit> gameOver =>
        from ws in winners
        from ps in playersState
        from _1 in Display.winners(ws)
        from _2 in Display.playerStates(ps)
        select unit;
}
