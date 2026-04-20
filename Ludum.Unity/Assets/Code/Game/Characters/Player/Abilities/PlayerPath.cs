using Code.Core.GameLoop;
using Code.Core.ServiceLocator;
using Code.Game.World;
using Code.Tools;

namespace Code.Game.Characters.Player.Abilities
{
    public class PlayerPath : ICharacterComponent, ISubscriber
    {
        private readonly PlayerView _view;
        private readonly PlayerInput _input;
        private readonly MachineSpawner _machineSpawner;
        private readonly PixelTrail _pixelTrail;
        public Condition Condition { get; } = new Condition();

        public PlayerPath(PlayerView view)
        {
            _view = view;
            _input = Container.Instance.GetService<PlayerInput>();
            _machineSpawner = Container.Instance.GetService<MachineSpawner>();
            _pixelTrail = Container.Instance.GetService<PixelTrail>();
        }
        
        public void Subscribe()
        {
            _input.PathPressed += _showPath;
        }

        public void Unsubscribe()
        {
            _input.PathPressed -= _showPath;
        }

        private void _showPath()
        {
            _pixelTrail.Activate(10,_view.transform.position, _machineSpawner.Machine.transform.position);
        }
    }
}