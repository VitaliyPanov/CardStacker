using CardStacker.General.Services;
using UnityEngine;
using Zenject;

namespace CardStacker.Core
{
    public class Bootstrapper : MonoBehaviour, IBootstrapper
    {
        [SerializeField] private LoadingScreen _loadingScreenPrefab;
        private SceneContext _bootstrapContext;
        private Game _game;

        private void Awake()
        {
            CreateServices();
            DontDestroyOnLoad(this);
        }

        [Inject]
        private void OnContextRun(IGameFactory gameFactory)
        {
            _game = new Game(Instantiate(_loadingScreenPrefab), gameFactory, this);
            _game.Start();
        }

        private void CreateServices()
        {
            _bootstrapContext = GetComponent<SceneContext>();
            _bootstrapContext.Run();
        }
        public void Remove()
        {
            var projectContext = Object.FindObjectOfType<ProjectContext>().gameObject;
            Destroy(projectContext);
            Destroy(gameObject);
        }
    }
}