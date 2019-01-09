using NUnit.Framework;
using UnitySnake;
using UnityEngine;

namespace Tests.NoECS
{
    public class PlayerInputTests
    {
        public class ConstructorTests
        {
            [Test]
            public void Null_InputSource_Throws_ArgumentNullException()
            {
                Assert.That(() => new PlayerInput(null), Throws.ArgumentNullException);
            }
        }

        public class GetInputDirectionTests
        {
            [Test]
            public void Input_W_Or_UpArrow_Returns_Up()
            {
                Assert.That(new PlayerInput(new InputSourceMock(KeyCode.W)).GetInputDirection(), Is.EqualTo(Direction.Up));
                Assert.That(new PlayerInput(new InputSourceMock(KeyCode.UpArrow)).GetInputDirection(), Is.EqualTo(Direction.Up));
            }

            [Test]
            public void Input_S_Or_DownArrow_Returns_Down()
            {
                Assert.That(new PlayerInput(new InputSourceMock(KeyCode.S)).GetInputDirection(), Is.EqualTo(Direction.Down));
                Assert.That(new PlayerInput(new InputSourceMock(KeyCode.DownArrow)).GetInputDirection(), Is.EqualTo(Direction.Down));
            }

            [Test]
            public void Input_A_Or_LeftArrow_Returns_Left()
            {
                Assert.That(new PlayerInput(new InputSourceMock(KeyCode.A)).GetInputDirection(), Is.EqualTo(Direction.Left));
                Assert.That(new PlayerInput(new InputSourceMock(KeyCode.LeftArrow)).GetInputDirection(), Is.EqualTo(Direction.Left));
            }

            [Test]
            public void Input_D_Or_RightArrow_Returns_Right()
            {
                Assert.That(new PlayerInput(new InputSourceMock(KeyCode.D)).GetInputDirection(), Is.EqualTo(Direction.Right));
                Assert.That(new PlayerInput(new InputSourceMock(KeyCode.RightArrow)).GetInputDirection(), Is.EqualTo(Direction.Right));
            }
        }

        private class InputSourceMock : IInputSource
        {
            public KeyCode LastPressedKey { get; private set; }

            public InputSourceMock(KeyCode lastPressedKey)
            {
                LastPressedKey = lastPressedKey;
            }

            public void Update() { }
        }
    }
}
