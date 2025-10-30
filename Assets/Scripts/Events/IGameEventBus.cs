
using System.Collections.Generic;


/// <summary>
/// Marker interface for all events.
/// </summary>
public interface IGameEvent { }

[DontLogEvent]
public struct BallReturnedEvent : IGameEvent
{
  public BallBase BallBase;
  public BallReturnedEvent(BallBase ballBase) => BallBase = ballBase;
}

[DontLogEvent]
public struct BallFiredEvent : IGameEvent
{
  public BallBase BallBase;
  public BallFiredEvent(BallBase ballBase) => BallBase = ballBase;
}

[DontLogEvent]
public struct BallCountChangedEvent : IGameEvent
{
  public int CurrentBallCount;
  public BallCountChangedEvent(int currentBallCount) => CurrentBallCount = currentBallCount;
}

public struct PickupBallEvent : IGameEvent
{
}
public struct PickupBoxEvent : IGameEvent
{
}

public struct PickupCollectedEvent : IGameEvent
{
  public IPickupable Pickup;
  public PickupCollectedEvent(IPickupable pickup) => Pickup = pickup;
}

[DontLogEvent]
public struct BoardObjectDeathEvent : IGameEvent
{
  public BoardObject BoardObject;
  public BoardObjectDeathEvent(BoardObject boardObject) => BoardObject = boardObject;
}

public struct AllBallShotEvent : IGameEvent
{
}

public struct AllBallReturnedEvent : IGameEvent
{
  public List<BallBase> ReturnedBalls;
  public AllBallReturnedEvent(List<BallBase> returnedBalls) => ReturnedBalls = returnedBalls;
}

public struct PlayerCanShootEvent : IGameEvent
{
  public bool CanShoot;
  public PlayerCanShootEvent(bool canShoot) => CanShoot = canShoot;
}


public struct SkillSelectedEvent : IGameEvent
{
  public PlayerSkillSO PlayerSkillSO;

  public SkillSelectedEvent(PlayerSkillSO playerSkillSO)
  {
    PlayerSkillSO = playerSkillSO;
  }
}

public struct SkillSelectionCompleteEvent : IGameEvent
{
}

public struct ResetSkillsEvent : IGameEvent
{
}