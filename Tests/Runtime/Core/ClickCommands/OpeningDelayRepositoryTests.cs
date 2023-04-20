using NUnit.Framework;
using Agava.Merge2.Tests;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.TestTools;
using System.Collections;
using System.Linq;

namespace Agava.Merge2.Core.Tests.ClickCommandsTests
{
    public class OpeningDelayRepositoryTests
    {
        private OpeningDelayRepository _delayRepository;
        private OpeningDelaySettings _settings;
        private IJsonSaveRepository _saveRepository;

        [SetUp]
        public void Initialize()
        {
            _settings = new OpeningDelaySettings(new[]
            {
                new KeyValuePair<Item, int>(new Item("Item1"), 1),
                new KeyValuePair<Item, int>(new Item("Item1", 1), 2),
                new KeyValuePair<Item, int>(new Item("Item2"), 2),
                new KeyValuePair<Item, int>(new Item("Item3"), 3),
            });

            _saveRepository = new MemoryJsonSaveRepository();
            _delayRepository = new OpeningDelayRepository(_settings, new LocalTimeProvider(), _saveRepository);
        }

        [Test]
        public void ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new OpeningDelayRepository(null, new LocalTimeProvider(), new MemoryJsonSaveRepository()));
            Assert.Throws<ArgumentException>(() => new OpeningDelayRepository(_settings, null, new MemoryJsonSaveRepository()));
            Assert.Throws<ArgumentException>(() => new OpeningDelayRepository(_settings, new LocalTimeProvider(), null));
        }

        [UnityTest]
        public IEnumerator CanClickTests()
        {
            Assert.True(_delayRepository.Completed(new Item("Item")));
            Assert.True(_delayRepository.Completed(new Item("Item1", 2)));
            Assert.False(_delayRepository.Completed(new Item("Item1")));

            var item1 = new Item("Item1");
            var item1_1 = new Item("Item1", 1);

            _delayRepository.Open(item1);
            _delayRepository.Open(item1_1);

            Assert.False(_delayRepository.Completed(item1));
            Assert.False(_delayRepository.Completed(item1_1));

            yield return new WaitForSeconds(1.5f);

            Assert.True(_delayRepository.Completed(item1));
            Assert.False(_delayRepository.Completed(new Item("Item1")));

            yield return new WaitForSeconds(1f);

            Assert.True(_delayRepository.Completed(item1_1));
            Assert.False(_delayRepository.Completed(new Item("Item1", 1)));
        }

        [UnityTest]
        public IEnumerator RemainsTests()
        {
            Assert.AreEqual(TimeSpan.Zero, _delayRepository.Remains(new Item("Item2", 1)));
            Assert.AreEqual(TimeSpan.Zero, _delayRepository.Remains(new Item("Item5")));

            var item2 = new Item("Item2");
            var item3 = new Item("Item3");

            _delayRepository.Open(item2);
            _delayRepository.Open(item3);

            var remains = _delayRepository.Remains(item2);
            Assert.True(remains.TotalSeconds > 1 && remains.TotalSeconds <= 2);

            yield return new WaitForSeconds(2.1f);
            
            Assert.True(_delayRepository.Remains(item2).TotalSeconds == 0);
            remains = _delayRepository.Remains(item3);
            Assert.True(remains.TotalSeconds > 0 && remains.TotalSeconds < 1);
            
            yield return new WaitForSeconds(1f);

            Assert.True(_delayRepository.Remains(item3).Seconds == 0);
        }

        [Test]
        public void ShouldThrowIfOpenTwise()
        {
            var item = new Item("Item1");
            _delayRepository.Open(item);

            Assert.Throws<InvalidOperationException>(() => _delayRepository.Open(item));
        }

        [Test]
        public void ShouldThrowIfOpenWrongItem()
        {
            Assert.Throws<InvalidOperationException>(() => _delayRepository.Open(new Item("Item")));
        }

        [Test]
        public void ShouldThrowWhenCantRemove()
        {
            var item = new Item("Item1");
            Assert.Throws<InvalidOperationException>(() => _delayRepository.Remove(item));

            _delayRepository.Open(item);
            Assert.DoesNotThrow(() => _delayRepository.Remove(item));
        }

        [Test]
        public void ShouldMemorySave()
        {
            var item = new Item("Item1");
            _delayRepository.Open(item);

            var secondRepository = new OpeningDelayRepository(_settings, new LocalTimeProvider(), _saveRepository);
            secondRepository.Load();

            Assert.AreEqual(1, secondRepository.Items.Count);
            Assert.AreEqual(secondRepository.Items.ToArray()[0], item.Guid);
        }
    }
}
