using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Messaging;
using Moq;
using NUnit.Framework;
using TimeTraveler.Libary.Models;
using TimeTraveler.Libary.Services;
using TimeTraveler.Libary.ViewModels;

namespace TimeTraveler.UnitTest.ViewModels;

[TestFixture]
public class GameTwoViewModelTests
{
    private Mock<IMazeService> _mockMazeService;
    private Mock<IBuffStorage> _mockBuffStorage;
    private Mock<ILargeModelService> _mockLargeModelService;
    private GameTwoViewModel _viewModel;

    [SetUp]
    public void SetUp()
    {
        _mockMazeService = new Mock<IMazeService>();
        _mockBuffStorage = new Mock<IBuffStorage>();
        _mockLargeModelService = new Mock<ILargeModelService>();
        

        _mockMazeService.Setup(m => m.GetMazeRepresentation())
            .Returns(string.Join("\n", Enumerable.Repeat(new string('#', 15), 15)));

        _viewModel = new GameTwoViewModel(_mockMazeService.Object, _mockBuffStorage.Object,_mockLargeModelService.Object);
    }

    [Test]
    public void Constructor_InitializesPropertiesCorrectly()
    {
        Assert.That(_viewModel.RowCount, Is.EqualTo(15));
        Assert.That(_viewModel.ColumnCount, Is.EqualTo(15));
        Assert.That(_viewModel.MazeGrid, Is.Not.Null);
        Assert.That(_viewModel.MazeGrid, Is.InstanceOf<ObservableCollection<ObservableCollection<MazeCell>>>());
    }

  

    [Test]
    public void UpdateMaze_PopulatesMazeGridCorrectly()
    {
        _viewModel.UpdateMaze();

        Assert.That(_viewModel.MazeGrid.Count, Is.EqualTo(15));
        Assert.That(_viewModel.MazeGrid[0].Count, Is.EqualTo(15));
        Assert.That(_viewModel.MazeGrid[0][0].Character, Is.EqualTo("#"));
    }

    [Test]
    public void CanPassWall_ReturnsCorrectValue()
    {
        Assert.That(_viewModel.CanPassWall(' '), Is.True);
        Assert.That(_viewModel.CanPassWall('#'), Is.False);
    }

   

    

    
     [Test]
    public void ReplyProperty_SetAndGet_ReturnsCorrectValue()
    {
        // Act
        _viewModel.Reply = "Test Reply";

        // Assert
        Assert.That(_viewModel.Reply, Is.EqualTo("Test Reply"));
    }

    [Test]
    public void CurrentPuzzleProperty_SetAndGet_ReturnsCorrectValue()
    {
        // Arrange
        var puzzle = new Puzzle {  Question = "Test Question", CorrectAnswer = "Test Answer" };

        // Act
        _viewModel.CurrentPuzzle = puzzle;

        // Assert
        Assert.That(_viewModel.CurrentPuzzle, Is.EqualTo(puzzle));
    }

    [Test]
    public void PlayerAnswerProperty_SetAndGet_ReturnsCorrectValue()
    {
        // Act
        _viewModel.PlayerAnswer = "My Answer";

        // Assert
        Assert.That(_viewModel.PlayerAnswer, Is.EqualTo("My Answer"));
    }

    [Test]
    public void IsPopupVisibleProperty_SetAndGet_ReturnsCorrectValue()
    {
        // Act
        _viewModel.IsPopupVisuale = true;

        // Assert
        Assert.That(_viewModel.IsPopupVisuale, Is.True);
    }

    [Test]
    public void MazeProperty_SetAndGet_ReturnsCorrectValue()
    {
        // Act
        _viewModel.Maze = "Test Maze Representation";

        // Assert
        Assert.That(_viewModel.Maze, Is.EqualTo("Test Maze Representation"));
    }

    [Test]
    public void MazeGrid_InitializesCorrectly()
    {
        // Assert
        Assert.That(_viewModel.MazeGrid, Is.Not.Null);
        Assert.That(_viewModel.MazeGrid, Is.InstanceOf<ObservableCollection<ObservableCollection<MazeCell>>>());
    }

    [Test]
    public void RowAndColumnCount_HaveCorrectValues()
    {
        // Assert
        Assert.That(_viewModel.RowCount, Is.EqualTo(15));
        Assert.That(_viewModel.ColumnCount, Is.EqualTo(15));
    }

    [Test]
    public void GameStatusProperty_SetAndTriggerOnGameStatusChanged()
    {
        // Arrange
        var eventTriggered = false;
        _viewModel.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(_viewModel.GameStatus))
                eventTriggered = true;
        };

        // Act
        _viewModel.GameStatus = "New Status";

        // Assert
        Assert.That(_viewModel.GameStatus, Is.EqualTo("New Status"));
        Assert.That(eventTriggered, Is.True);
    }

    [Test]
    public void PlayerAttributes_InitializesCorrectly()
    {
        // Assert
        Assert.That(_viewModel._playerAttributes, Is.Not.Null);
        Assert.That(_viewModel._playerAttributes, Is.Empty);
    }
    
     [Test]
    public void WinStatus_SetAndTriggerOnWinStatusChanged()
    {
        // Arrange
        var receivedMessage = string.Empty;
        WeakReferenceMessenger.Default.Register<WinStatusMessage>(this, (r, m) => receivedMessage = m.Message);

        // Act
        _viewModel.WinStatus = "You Win!";

        // Assert
        Assert.That(receivedMessage, Is.EqualTo("You Win!"));
    }

    [Test]
    public void GameStatus_SetAndTriggerOnGameStatusChanged()
    {
        // Arrange
        var receivedMessage = string.Empty;
        WeakReferenceMessenger.Default.Register<GameStatusMessage>(this, (r, m) => receivedMessage = m.Message);

        // Act
        _viewModel.GameStatus = "Game Over";

        // Assert
        Assert.That(receivedMessage, Is.EqualTo("Game Over"));
    }

    [Test]
    public void IsPopupVisible_ChangeViableAndClose_SetCorrectly()
    {
        // Act
        _viewModel.changeviable();

        // Assert
        Assert.That(_viewModel.IsPopupVisuale, Is.True);

        // Act
        _viewModel.close();

        // Assert
        Assert.That(_viewModel.IsPopupVisuale, Is.False);
    }

    [Test]
    public void ChangeCommand_ExecutesCorrectly()
    {
        // Act
        _viewModel.ChangeCommand.Execute(null);

        // Assert
        Assert.That(_viewModel.IsPopupVisuale, Is.True);
    }

    [Test]
    public void ClosePopCommand_ExecutesCorrectly()
    {
        // Arrange
        _viewModel.changeviable(); // 显示弹窗

        // Act
        _viewModel.ClosePopCommand.Execute(null);

        // Assert
        Assert.That(_viewModel.IsPopupVisuale, Is.False);
    }

    [Test]
    public void MoveCommands_AreBoundCorrectly()
    {
        // Assert
        Assert.That(_viewModel.MoveUpCommand, Is.Not.Null);
        Assert.That(_viewModel.MoveDownCommand, Is.Not.Null);
        Assert.That(_viewModel.MoveLeftCommand, Is.Not.Null);
        Assert.That(_viewModel.MoveRightCommand, Is.Not.Null);
    }

    [Test]
    public void RestartCommand_IsBoundCorrectly()
    {
        // Assert
        Assert.That(_viewModel.RestartCommand, Is.Not.Null);
    }

    [Test]
    public void MoveCommands_ExecuteWithoutErrors()
    {
        // Act & Assert
        Assert.DoesNotThrow(() => _viewModel.MoveUpCommand.Execute(null));
        Assert.DoesNotThrow(() => _viewModel.MoveDownCommand.Execute(null));
        Assert.DoesNotThrow(() => _viewModel.MoveLeftCommand.Execute(null));
        Assert.DoesNotThrow(() => _viewModel.MoveRightCommand.Execute(null));
    }

    [Test]
    public void RestartCommand_ExecuteWithoutErrors()
    {
        // Act & Assert
        Assert.DoesNotThrow(() => _viewModel.RestartCommand.Execute(null));
    }
    
     [Test]
    public void GetPuzzle_WithTheme_SetsCurrentPuzzleAndShowsPopup()
    {
        // Arrange
        var theme = "Fire";
        var puzzle = new Puzzle
        {
            Question = "What is 2+2?",
            Options = new[] { "A. 3", "B. 4", "C. 5" },
            CorrectAnswer = "B",
            explanation = "2+2 equals 4."
        };

        _mockLargeModelService.Setup(service => service.GeneratePuzzle(theme)).Returns(puzzle);
        _viewModel.theme = theme;

        // Act
        _viewModel.GetPuzzle();

        // Assert
        Assert.That(_viewModel.CurrentPuzzle, Is.Not.Null);
        Assert.That(_viewModel.CurrentPuzzle.Question, Is.EqualTo("What is 2+2?"));
        Assert.That(_viewModel.IsPopupVisuale, Is.True);
    }

    [Test]
    public void GetPuzzle_WithoutTheme_DoesNotSetCurrentPuzzle()
    {
        // Act
        _viewModel.GetPuzzle();

        // Assert
        Assert.That(_viewModel.CurrentPuzzle, Is.Null);
        Assert.That(_viewModel.IsPopupVisuale, Is.False);
    }

    [TestCase("A")]
    [TestCase("B")]
    [TestCase("C")]
    public void ChooseAnswer_SetsPlayerAnswerAndChecksAnswer(string choice)
    {
        // Arrange
        _viewModel.CurrentPuzzle = new Puzzle
        {
            CorrectAnswer = "B",
            explanation = "Correct answer is B."
        };

        // Act
        switch (choice)
        {
            case "A":
                _viewModel.ChooseA();
                break;
            case "B":
                _viewModel.ChooseB();
                break;
            case "C":
                _viewModel.ChooseC();
                break;
        }

        // Assert
        Assert.That(_viewModel.PlayerAnswer, Is.EqualTo(choice));
    }



    [Test]
    public async Task CheckAnswer_WrongAnswer_DoesNotAddBuff()
    {
        // Arrange
        _viewModel.CurrentPuzzle = new Puzzle
        {
            CorrectAnswer = "B",
            explanation = "Correct answer is B."
        };
        _viewModel.PlayerAnswer = "A";

        // Act
        var result = await _viewModel.checkAnswer();

        // Assert
        Assert.That(result, Is.False);
        Assert.That(_viewModel.Reply, Does.Contain("回答错误"));
        _mockBuffStorage.Verify(storage => storage.AddBuffsAsync(It.IsAny<Buff>()), Times.Never);
    }
    



    [Test]
    public void StartNewGame_InitializesGameAndLoadsAttributes()
    {
        // Act
        _viewModel.StartNewGame();

        // Assert
        Assert.That(_viewModel.GameStatus, Is.EqualTo("游戏进行中..."));
        Assert.That(_viewModel._playerX, Is.EqualTo(1));
        Assert.That(_viewModel._playerY, Is.EqualTo(1));
        
    }
    
     

    [Test]
    public void UpdateMaze_WithValidMazeRepresentation_UpdatesMazeGrid()
    {
        // Arrange
        var mazeRepresentation = string.Join("\n", Enumerable.Repeat("###############", 15));
        _mockMazeService.Setup(service => service.GetMazeRepresentation()).Returns(mazeRepresentation);

        // Act
        _viewModel.UpdateMaze();

        // Assert
        Assert.That(_viewModel.MazeGrid.Count, Is.EqualTo(15));
        Assert.That(_viewModel.MazeGrid[0].Count, Is.EqualTo(15));
        Assert.That(_viewModel.MazeGrid[0][0].Character, Is.EqualTo("#"));
    }

    [Test]
    public void UpdateMaze_WithInvalidMazeRepresentation_SetsGameStatusToError()
    {
        // Arrange
        _mockMazeService.Setup(service => service.GetMazeRepresentation()).Returns(string.Empty);

        // Act
        _viewModel.UpdateMaze();

        // Assert
        Assert.That(_viewModel.GameStatus, Is.EqualTo("迷宫加载失败！"));
    }

    [Test]
    public void MakeMove_PlayerCannotPassWall_UpdatesGameStatus()
    {
        // Arrange
        var mazeRepresentation = string.Join("\n", Enumerable.Repeat("###############", 15));
        _mockMazeService.Setup(service => service.GetMazeRepresentation()).Returns(mazeRepresentation);

        _viewModel._playerX = 1;
        _viewModel._playerY = 1;

        // Act
        _viewModel.MoveRight();

        // Assert
        Assert.That(_viewModel.GameStatus, Is.EqualTo("墙壁不可通过"));
        Assert.That(_viewModel._playerX, Is.EqualTo(1));
        Assert.That(_viewModel._playerY, Is.EqualTo(1));
    }

    [Test]
    public void MakeMove_PlayerCanPassWall_UpdatesPlayerPosition()
    {
        // Arrange
        var mazeRepresentation = string.Join("\n", Enumerable.Repeat("#   ###########", 15));
        _mockMazeService.Setup(service => service.GetMazeRepresentation()).Returns(mazeRepresentation);

        _viewModel._playerX = 1;
        _viewModel._playerY = 1;

        // Act
        _viewModel.MoveRight();

        // Assert
        Assert.That(_viewModel._playerX, Is.EqualTo(2));
        Assert.That(_viewModel._playerY, Is.EqualTo(1));
    }

  

    [Test]
    public void MakeMove_PlayerCannotMoveOutsideMaze_BoundsCheckPreventsMove()
    {
        // Arrange
        _viewModel._playerX = 0;
        _viewModel._playerY = 0;

        // Act
        _viewModel.MoveUp();

        // Assert
        Assert.That(_viewModel._playerX, Is.EqualTo(0));
        Assert.That(_viewModel._playerY, Is.EqualTo(0));
    }

 [Test]
    public async Task QuitCommand_ExecutesProperly()
    {
        // Arrange
    

        // Act
        _viewModel.Quit();

        // Assert
        Assert.That(_viewModel.isOK, Is.False);
        Assert.That(_viewModel.GameStatus, Is.EqualTo("游戏进行中..."));
    }

    [Test]
    public void Restart_ResetsGameProperly()
    {
        // Arrange
        _viewModel._playerX = 5;
        _viewModel._playerY = 5;


        // Act
        _viewModel.Restart();

        // Assert
        Assert.That(_viewModel.isOK, Is.False);
      
        Assert.That(_viewModel._playerX, Is.EqualTo(1));
        Assert.That(_viewModel._playerY, Is.EqualTo(1));
        Assert.That(_viewModel.GameStatus, Is.EqualTo("游戏进行中..."));
    }

    [Test]
    public void GoToResultView_SubmitsCorrectResult_WhenIsOKIsTrue()
    {
        // Arrange
        _viewModel.isOK = true;
        var messengerMock = new Mock<IMessenger>();
        
        // Act
        _viewModel.GoToResultView();

     
        Assert.That(_viewModel.isOK, Is.False); // 游戏被重置
    }
    

    [Test]
    public void MazeCell_StoresAndRetrievesValuesCorrectly()
    {
        // Arrange
        var mazeCell = new MazeCell
        {
            Character = "F",
            Image = null,
            Row = 3,
            Column = 4
        };

        // Assert
        Assert.That(mazeCell.Character, Is.EqualTo("F"));
        Assert.That(mazeCell.Image, Is.Null);
        Assert.That(mazeCell.Row, Is.EqualTo(3));
        Assert.That(mazeCell.Column, Is.EqualTo(4));
    }

    [Test]
    public void WinStatusMessage_StoresAndRetrievesValuesCorrectly()
    {
        // Arrange
        var winMessage = new WinStatusMessage { Message = "Test Win Message" };

        // Assert
        Assert.That(winMessage.Message, Is.EqualTo("Test Win Message"));
    }

    [Test]
    public void GameStatusMessage_StoresAndRetrievesValuesCorrectly()
    {
        // Arrange
        var gameStatusMessage = new GameStatusMessage { Message = "Test Game Status" };

        // Assert
        Assert.That(gameStatusMessage.Message, Is.EqualTo("Test Game Status"));
    }

    [Test]

    public void CheckChooseA()
    {
        var viewModelChooseACommand = _viewModel.ChooseACommand;
        viewModelChooseACommand.Execute(null);
        Assert.That(this._viewModel.PlayerAnswer.Equals("A"));
    }
    
    [Test]

    public void CheckChooseC()
    {
        var viewModelChooseACommand = _viewModel.ChooseCCommand;
        viewModelChooseACommand.Execute(null);
        Assert.That(this._viewModel.PlayerAnswer.Equals("C"));
    }
    
    
    [Test]

    public void CheckChooseB()
    {
        var viewModelChooseACommand = _viewModel.ChooseBCommand;
        viewModelChooseACommand.Execute(null);
        Assert.That(this._viewModel.PlayerAnswer.Equals("B"));
    }
}