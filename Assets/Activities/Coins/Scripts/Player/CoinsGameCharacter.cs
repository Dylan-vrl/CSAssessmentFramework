using CSFramework.Presettables;

namespace Player
{
    /// <summary>
    /// TODO
    /// </summary>
    public class CoinsGameCharacter: GameCharacter
    {
        protected override void Start()
        {
           base.Start();
           CoinsController.Instance.LastCollectiblePos = _spawnPos;
        }

        public override void Respawn(SpawnPoint spawnPoint = SpawnPoint.AtSpawn)
        {
            base.Respawn(spawnPoint);

            if (_activeTerrain == null) return;
            
            var pos = transform.position;
            if (spawnPoint == SpawnPoint.AtPath)
            {
                pos = CoinsController.Instance.LastCollectiblePos;
            }
            pos.y = _activeTerrain.SampleHeight(pos) + _activeTerrain.transform.position.y + 0.2f;
            transform.position = pos;
        }
    }
}