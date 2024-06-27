using R2API;
using RoR2;
using RoR2.ContentManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace LemurianFix.Content
{
    internal class ContentPackProvider : IContentPackProvider
    {
        readonly ContentPack _contentPack = new ContentPack();

        public string identifier { get; } = LemurianFixPlugin.PluginGUID;

        internal ContentPackProvider()
        {
        }

        internal void Register()
        {
            ContentManager.collectContentPackProviders += addContentPackProvider =>
            {
                addContentPackProvider(this);
            };
        }

        public IEnumerator LoadStaticContentAsync(LoadStaticContentAsyncArgs args)
        {
            _contentPack.identifier = identifier;

            const int NUM_LOAD_OPERATIONS = 2;

            AsyncOperationHandle<GameObject> lemurianBodyLoad = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Lemurian/LemurianBody.prefab");
            while (!lemurianBodyLoad.IsDone)
            {
                args.ReportProgress(Util.Remap(lemurianBodyLoad.PercentComplete, 0f, 1f, 0f, 1f / NUM_LOAD_OPERATIONS));
                yield return 0;
            }

            AsyncOperationHandle<GameObject> devotedLemurianMasterLoad = Addressables.LoadAssetAsync<GameObject>("RoR2/CU8/LemurianEgg/DevotedLemurianMaster.prefab");
            while (!devotedLemurianMasterLoad.IsDone)
            {
                args.ReportProgress(Util.Remap(devotedLemurianMasterLoad.PercentComplete, 0f, 1f, 2f / NUM_LOAD_OPERATIONS, 1f));
                yield return 0;
            }

            GameObject lemurianBodyPrefab = lemurianBodyLoad.Result;
            GameObject devotedLemurianMasterPrefab = devotedLemurianMasterLoad.Result;

            GameObject devotedLemurianBodyPrefab = lemurianBodyPrefab.InstantiateClone("DevotedLemurianBody", true);

            if (devotedLemurianBodyPrefab.TryGetComponent(out DeathRewards deathRewards))
            {
                deathRewards.logUnlockableDef = null;
            }

            _contentPack.bodyPrefabs.Add([
                devotedLemurianBodyPrefab
            ]);

            CharacterMaster devotedLemurianMaster = devotedLemurianMasterPrefab.GetComponent<CharacterMaster>();
            devotedLemurianMaster.bodyPrefab = devotedLemurianBodyPrefab;

            CharacterBody lemurianBody = lemurianBodyPrefab.GetComponent<CharacterBody>();
            lemurianBody.bodyFlags &= ~CharacterBody.BodyFlags.Devotion;

            args.ReportProgress(1f);
        }

        public IEnumerator GenerateContentPackAsync(GetContentPackAsyncArgs args)
        {
            ContentPack.Copy(_contentPack, args.output);
            args.ReportProgress(1f);
            yield break;
        }

        public IEnumerator FinalizeAsync(FinalizeAsyncArgs args)
        {
            yield break;
        }
    }
}
