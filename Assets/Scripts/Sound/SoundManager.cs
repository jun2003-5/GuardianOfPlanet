using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;


    public AudioSource musicSource;
    public AudioSource SFXSource;
    public AudioSource VictoryDefeatSource;

    [Header("#----Weapon SFXs")]
    public AudioSource BomberSFXSource;
    public AudioSource strLaserSFXSource;
    public AudioSource laserSFXSource;
    public AudioSource PoisonSFXSource;
    public AudioSource SniperSFXSource;

    [Header("#---Enemy Source")]
    public AudioSource EnemyDamageSource;

    [Header("#-----Music Clips")]
    [Space(5)]
    public AudioClip loadingScreen;
    public AudioClip MainTitle;
    public AudioClip MineBGM;
    public AudioClip WorldMapBGM;
    public AudioClip AdventureBattleBGM;
    public AudioClip DungeonBGM;
    public AudioClip DungeonMapBGM;

    [Header("#-----SFX Clips")]
    [Space(5)]
    public AudioClip click;
    public AudioClip coin;
    public AudioClip achieve;
    public AudioClip mining;
    public AudioClip BuffBought;
    public AudioClip Victory;
    public AudioClip Defeat;

    [Header("#----Weapon SFX Clips")]
    public AudioClip BomberExplosionSFX;
    public AudioClip strLaserSFX;
    public AudioClip laserSFX;
    public AudioClip PoisonSFX;
    public AudioClip SniperSFX;

    [Header("#----Enemy Damage Clip")]
    public AudioClip EnemyDamagedSFX;
    public AudioClip EnemyDeadSFX;

    public enum MusicType { loadingScreen, MainTitle, MineBGM, WorldMapBGM, AdventureBattleBGM, DungeonBGM, DungeonMapBGM};

    public enum SFXType {Click, Coin, LevelUp, Victory, Defeat, achieve, BuffBought};

    public enum WeaponSFXType { BomberExplosion, strLaser, laser, poison, Sniper};

    private void Awake()
    {
        Instance = this;
    }

    public void playMusic(MusicType type)
    {
        switch(type) {
            case MusicType.loadingScreen:
                musicSource.clip = loadingScreen;
                musicSource.Play();
                break;
            case MusicType.MainTitle:
                musicSource.clip = MainTitle;
                musicSource.Play();
                break;
            case MusicType.MineBGM:
                musicSource.clip = MineBGM;
                musicSource.Play();
                break;
            case MusicType.WorldMapBGM:
                musicSource.clip = WorldMapBGM;
                musicSource.Play();
                break;
            case MusicType.AdventureBattleBGM:
                musicSource.clip = AdventureBattleBGM;
                musicSource.Play();
                break;
            case MusicType.DungeonBGM:
                musicSource.clip = DungeonBGM;
                musicSource.Play();
                break;
            case MusicType.DungeonMapBGM:
                musicSource.clip = DungeonMapBGM;
                musicSource.Play();
                break;
        }
    }

    public void playSFX(SFXType type)
    {
        switch(type) {
            case SFXType.Click:
                SFXSource.clip = click;
                SFXSource.Play();
                break;
            case SFXType.Coin:
                SFXSource.clip = coin;
                SFXSource.Play();
                break;
            case SFXType.achieve:
                SFXSource.clip = achieve;
                SFXSource.Play();
                break;
            case SFXType.BuffBought:
                SFXSource.clip = BuffBought;
                SFXSource.Play();
                break;
        }
    }

    public void playWeaponSFX(WeaponSFXType type)
    {
        switch(type) {
            case WeaponSFXType.BomberExplosion:
                BomberSFXSource.clip = BomberExplosionSFX;
                BomberSFXSource.Play();
                break;
            case WeaponSFXType.strLaser:
                strLaserSFXSource.clip = strLaserSFX;
                strLaserSFXSource.Play();
                break;
            case WeaponSFXType.laser:
                laserSFXSource.clip = laserSFX;
                laserSFXSource.Play();
                break;
            case WeaponSFXType.poison:
                PoisonSFXSource.clip = PoisonSFX;
                PoisonSFXSource.Play();
                break;
            case WeaponSFXType.Sniper:
                SniperSFXSource.clip = SniperSFX;
                SniperSFXSource.Play();
                break;
        }
    }

    public void playVictory()
    {
        VictoryDefeatSource.clip = Victory;
        VictoryDefeatSource.Play();
    }

    public void playDefeat()
    {
        VictoryDefeatSource.clip = Defeat;
        VictoryDefeatSource.Play();
    }

    public void playEnemyHit()
    {
        EnemyDamageSource.clip = EnemyDamagedSFX;
        EnemyDamageSource.Play();
    }

    public void playEnemyDead()
    {
        EnemyDamageSource.clip = EnemyDeadSFX;
        EnemyDamageSource.Play();
    }

    public void playClickSFX()
    {
        SFXSource.clip = click;
        SFXSource.Play();
    }

    public void playCoinSFX()
    {
        SFXSource.clip = coin;
        SFXSource.Play();
    }

    public void playAchieveSFX()
    {
        SFXSource.clip = achieve;
        SFXSource.Play();
    }

    public void playMiningSFX()
    {
        SFXSource.clip = mining;
        SFXSource.Play();
    }

    public void playBuffSFX()
    {
        SFXSource.clip = BuffBought;
        SFXSource.Play();
    }

    //Music
    public void playMainTitleMusic()
    {
        musicSource.clip = MainTitle;
        musicSource.Play();
    }
    public void playMapMusic()
    {
        musicSource.clip = WorldMapBGM;
        musicSource.Play();
    }

    public void playDungeonMapMusic()
    {
        musicSource.clip = DungeonMapBGM;
        musicSource.Play();
    }
}
