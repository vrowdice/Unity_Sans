using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적 공격 타입 이넘
/// 데이터 상에서는 각 인덱스를 부여받음
/// </summary>
public enum AtkType
{
    Simple, //1 index 단순 공격
    BlueSimple, //2 index 움직일 경우 공격
    Pop, //3 index 경고 후 튀어오르며 단순 공격
    Range, //4 index 일직선 원거리 공격
    Gravity, //5 index 중력 공격 (데미지 X)
    Scaffold, //6 index 발판 생성
    GuidRange, //7 index 원거리 유도 공격
    ResetPosition //8 index 위치를 기본 위치로 초기화
}
