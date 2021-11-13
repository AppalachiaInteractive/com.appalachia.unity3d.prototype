using Appalachia.Audio.Contextual.Context.Collections;
using Appalachia.Audio.Contextual.Context.Contexts;
using Appalachia.Audio.Contextual.Execution;

namespace Appalachia.Prototype.KOC.Character.Audio.Execution
{
    public abstract class CharacterAudioExecutionProcessor<TCollection, TContext, TParams> :
        AudioExecutionProcessor<TCollection, TContext, TParams, CharacterAudioExecutionManagerBehaviour>
        where TCollection : AudioContextCollection<TContext, TParams, TCollection>
        where TContext : AudioContext<TParams>
        where TParams : AudioContextParameters, new()
    {
        public virtual void OnDie(
            PlayerCharacter player,
            CharacterAudioExecutionManagerBehaviour audioManager)
        {
        }

        public virtual void OnInWater_End(
            PlayerCharacter player,
            CharacterAudioExecutionManagerBehaviour audioManager)
        {
        }

        public virtual void OnInWater_Start(
            PlayerCharacter player,
            CharacterAudioExecutionManagerBehaviour audioManager)
        {
        }

        public virtual void OnJump(
            PlayerCharacter player,
            CharacterAudioExecutionManagerBehaviour audioManager)
        {
        }

        public virtual void OnLand(
            PlayerCharacter player,
            CharacterAudioExecutionManagerBehaviour audioManager)
        {
        }

        public virtual void OnSleeping_End(
            PlayerCharacter player,
            CharacterAudioExecutionManagerBehaviour audioManager)
        {
        }

        public virtual void OnSleeping_Start(
            PlayerCharacter player,
            CharacterAudioExecutionManagerBehaviour audioManager)
        {
        }

        public virtual void OnStep(
            PlayerCharacter player,
            CharacterAudioExecutionManagerBehaviour audioManager)
        {
        }

        public virtual void OnSwimming_End(
            PlayerCharacter player,
            CharacterAudioExecutionManagerBehaviour audioManager)
        {
        }

        public virtual void OnSwimming_Start(
            PlayerCharacter player,
            CharacterAudioExecutionManagerBehaviour audioManager)
        {
        }

        public virtual void OnUnderWater_End(
            PlayerCharacter player,
            CharacterAudioExecutionManagerBehaviour audioManager)
        {
        }

        public virtual void OnUnderWater_Start(
            PlayerCharacter player,
            CharacterAudioExecutionManagerBehaviour audioManager)
        {
        }

        public virtual void OnVocalize_End(
            PlayerCharacter player,
            CharacterAudioExecutionManagerBehaviour audioManager)
        {
        }

        public virtual void OnVocalize_Start(
            PlayerCharacter player,
            CharacterAudioExecutionManagerBehaviour audioManager)
        {
        }
    }
}
