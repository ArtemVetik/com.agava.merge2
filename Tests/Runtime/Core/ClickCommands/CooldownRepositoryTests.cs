using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using Agava.Merge2.Tests;
using System.Linq;

namespace Agava.Merge2.Core.Tests.ClickCommandsTests
{
    public class CooldownRepositoryTests
    {
        private CooldownSettings _settings;
        private CooldownRepository _repository;
        private IJsonSaveRepository _save;

        [SetUp]
        public void Initialize()
        {
            _settings = new CooldownSettings(new[]
            {
                new KeyValuePair<Item, Cooldown>(new Item("Item1"), new Cooldown(5, 2)),
                new KeyValuePair<Item, Cooldown>(new Item("Item1",1), new Cooldown(3, 3)),
                new KeyValuePair<Item, Cooldown>(new Item("Item2"), new Cooldown(10, 1)),
                new KeyValuePair<Item, Cooldown>(new Item("Item3"), new Cooldown(2, 2)),
            });

            _save = new MemoryJsonSaveRepository();
            _repository = new CooldownRepository(_settings, new LocalTimeProvider(), _save);
        }

        [Test]
        public void ShouldThrowIfNullArgumentsInConstructor()
        {
            Assert.Throws<ArgumentNullException>(() => new CooldownRepository(null, new LocalTimeProvider(), new MemoryJsonSaveRepository()));
            Assert.Throws<ArgumentNullException>(() => new CooldownRepository(_settings, null, new MemoryJsonSaveRepository()));
            Assert.Throws<ArgumentNullException>(() => new CooldownRepository(_settings, new LocalTimeProvider(), null));
        }

        [UnityTest]
        public IEnumerator ShouldAddWithoutException()
        {
            var firstItem1 = new Item("Item1");
            var secondItem1 = new Item("Item1");
            var item1_2 = new Item("Item1", 1);

            AssertDoesNotThrow();
            yield return new WaitForSeconds(4f);
            AssertDoesNotThrow();

            void AssertDoesNotThrow()
            {
                Assert.DoesNotThrow(() =>
                {
                    for (int i = 0; i < 5; i++)
                    {
                        _repository.AddClick(firstItem1);
                        _repository.AddClick(secondItem1);
                    }

                    for (int i = 0; i < 3; i++)
                        _repository.AddClick(item1_2);
                });
            }
        }

        [Test]
        public void ShouldAddWithException()
        {
            var firstItem1 = new Item("Item1");
            var secondItem1 = new Item("Item1");
            var item1_2 = new Item("Item1", 1);

            for (int i = 0; i < 5; i++)
                _repository.AddClick(firstItem1);
            for (int i = 0; i < 5; i++)
                _repository.AddClick(secondItem1);
            for (int i = 0; i < 3; i++)
                _repository.AddClick(item1_2);

            Assert.Throws<InvalidOperationException>(() => _repository.AddClick(firstItem1));
            Assert.Throws<InvalidOperationException>(() => _repository.AddClick(secondItem1));
            Assert.Throws<InvalidOperationException>(() => _repository.AddClick(item1_2));
        }

        [Test]
        public void ShouldRemoveClickIfCan()
        {
            var item = new Item("Item1");
            Assert.Throws<InvalidOperationException>(() => _repository.RemoveClick(item));

            _repository.AddClick(item);
            Assert.DoesNotThrow(() => _repository.RemoveClick(item));
            Assert.AreEqual(0, _repository.CooldownItems.Count);
            
            _repository.AddClick(item);
            _repository.AddClick(item);
            Assert.DoesNotThrow(() => _repository.RemoveClick(item));
            Assert.AreEqual(1, _repository.CooldownItems.Count);
            Assert.DoesNotThrow(() => _repository.RemoveClick(item));
            Assert.AreEqual(0, _repository.CooldownItems.Count);
            Assert.Throws<InvalidOperationException>(() => _repository.RemoveClick(item));
        }

        [UnityTest]
        public IEnumerator ShouldReturnCorrectCompleteStatus()
        {
            var item = new Item("Item3");
            Assert.True(_repository.Completed(item));

            _repository.AddClick(item);
            Assert.True(_repository.Completed(item));

            _repository.AddClick(item);
            Assert.False(_repository.Completed(item));

            yield return new WaitForSeconds(3f);
            Assert.True(_repository.Completed(item));
        }

        [UnityTest]
        public IEnumerator ShouldReturnCorrectRemainingTime()
        {
            var item = new Item("Item3");
            Assert.AreEqual(0, _repository.Remains(item).TotalSeconds);

            _repository.AddClick(item);
            Assert.That(_repository.Remains(item).TotalSeconds > 0 && _repository.Remains(item).TotalSeconds <= 2);

            _repository.AddClick(item);
            Assert.That(_repository.Remains(item).TotalSeconds > 0 && _repository.Remains(item).TotalSeconds <= 2);

            yield return new WaitForSeconds(3f);
            Assert.That(_repository.Remains(item).TotalSeconds == 0);
        }

        [Test]
        public void ShouldSaveCooldownItems()
        {
            var item1 = new Item("Item1");
            var item1_2 = new Item("Item1", 1);

            _repository.AddClick(item1);
            _repository.AddClick(item1_2);

            var secondRepository = new CooldownRepository(_settings, new LocalTimeProvider(), _save);
            secondRepository.Load();

            Assert.AreEqual(2, secondRepository.CooldownItems.Count);
            Assert.True(secondRepository.CooldownItems.Any(guid => item1.Guid == guid));
            Assert.True(secondRepository.CooldownItems.Any(guid => item1_2.Guid == guid));
        }
    }
}
