using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests
{
    [TestFixture]
    public class StackTests
    {
        private Stack<int?> _stack;

        [SetUp]
        public void Setup()
        {
            _stack = new Stack<int?>();
        }

        [Test]
        public void Count_EmptyStack_ReturnsZero()
        {
            Assert.That(_stack.Count, Is.EqualTo(0));
        }

        [Test]
        public void Push_ValidArgument_AddValueToStack()
        {
            _stack.Push(1);

            Assert.That(_stack.Count, Is.EqualTo(1));
        }

        [Test]
        public void Push_ArgumentIsNull_ThrowsArgumentNullException()
        {
            Assert.That(()=> _stack.Push(null), Throws.ArgumentNullException);
        }

        [Test]
        public void Pop_WithElements_ReturnsObjectOnTop()
        {
            _stack.Push(1);
            _stack.Push(2);
            _stack.Push(3);

            var result = _stack.Pop();

            Assert.That(result, Is.EqualTo(3));
        }

        [Test]
        public void Pop_WithElements_RemovesObjectOnTop()
        {
            _stack.Push(1);
            _stack.Push(2);
            _stack.Push(3);
            _stack.Pop();

            var result = _stack.Pop();

            Assert.That(result, Is.EqualTo(2));
        }

        [Test]
        public void Pop_EmptyStack_ThrowsInvalidOperationException()
        {
            Assert.That(() => _stack.Pop(), Throws.InvalidOperationException);
        }

        [Test]
        public void Peek_WhenExistsElement_ReturnsElement()
        {
            _stack.Push(1);
            _stack.Push(2);
            _stack.Push(3);

            var result = _stack.Peek();

            Assert.That(result, Is.EqualTo(3));
        }

        [Test]
        public void Peek_WhenExistsElement_DoesNotRemoveElements()
        {
            _stack.Push(1);
            _stack.Push(2);
            _stack.Push(3);

            _stack.Peek();

            Assert.That(_stack.Count, Is.EqualTo(3));
        }

        [Test]
        public void Peek_EmptyStack_ThrowsInvalidOperationException()
        {
            Assert.That(() => _stack.Peek(), Throws.InvalidOperationException);
        }
    }
}
