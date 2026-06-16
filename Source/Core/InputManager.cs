using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace KingdomOfDarkness.Core;

public class InputManager
{
    private KeyboardState _currentKeyboardState;
    private KeyboardState _previousKeyboardState;
    
    private MouseState _currentMouseState;
    private MouseState _previousMouseState;

    private DisplayScaler _displayScaler;

    public Vector2 MovementIntent { get; private set; }
    public bool IsAttackRequested { get; private set; }

    public InputManager(DisplayScaler displayScaler)
    {
        _displayScaler = displayScaler;
    }

    public void Update()
    {
        _previousKeyboardState = _currentKeyboardState;
        _currentKeyboardState = Keyboard.GetState();

        _previousMouseState = _currentMouseState;
        _currentMouseState = Mouse.GetState();

        UpdateMovementIntent();
        UpdateActions();
    }

    private void UpdateMovementIntent()
    {
        float dx = 0f;
        float dy = 0f;

        // Strict Single-Axis Movement (Diagonal on screen)
        if (_currentKeyboardState.IsKeyDown(Keys.Up))
        {
            dy -= 1f;
        }
        else if (_currentKeyboardState.IsKeyDown(Keys.Down))
        {
            dy += 1f;
        }
        else if (_currentKeyboardState.IsKeyDown(Keys.Left))
        {
            dx -= 1f;
        }
        else if (_currentKeyboardState.IsKeyDown(Keys.Right))
        {
            dx += 1f;
        }

        MovementIntent = new Vector2(dx, dy);
    }

    private void UpdateActions()
    {
        // Attack is requested if Space is pressed this frame but wasn't last frame, or is currently held.
        // Let's make it simple: Space key triggers attack.
        IsAttackRequested = _currentKeyboardState.IsKeyDown(Keys.Space);
    }

    public bool IsKeyPressed(Keys key)
    {
        return _currentKeyboardState.IsKeyDown(key) && _previousKeyboardState.IsKeyUp(key);
    }

    public bool IsLeftMouseClicked()
    {
        return _currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released;
    }

    public Vector2 MousePosition => _displayScaler.ActualToVirtual(new Vector2(_currentMouseState.X, _currentMouseState.Y));
}
