# Unity 3D

## 조작

| WASD | 이동 |
| --- | --- |
| Space | 점프 |
| E | 아이템 사용 |
| Q | 아이템 버리기 |
| 마우스 좌클릭 | 상호작용 |
| Tab + WASD | 달리기 |
| 마우스 휠 | 인벤토리 내 아이템 전환 |
| V | 시점 전환 |

## 구현 기능

### 🤸기본 이동 및 점프

Input System과 ForceMode로 이동 및 점프를 구현했습니다. 점프는 지정된 LayerMask에서만 가능하게 했습니다.

달리거나 벽을 탈 때는 스테미나가 감소합니다.

### ⏹️UI

플레이어 상태에는 체력과 스테미나가 있습니다. 화면 아래에 스테미나 등을 사용하면 소모되도록 구현했습니다.

인벤토리는 소모 아이템을 획득하면 활성화되어 화면에 보이도록 했습니다. 마우스 휠로 아이템 슬롯을 변경할 수 있습니다.

### 🛏️플랫폼

점프 플랫폼, 움직이는 플랫폼, 발사 플랫폼이 있고, ForceMode, Coroutoine 등으로 구현했습니다.

### 🍎아이템

소비 아이템, 즉발 아이템이 있습니다. 소비 아이템은 획득하여 인벤토리에 저장하여 사용하거나 버릴 수 있습니다. 체력을 지속적으 회복하거나 감소시킬 수 있습니다. 

즉발 아이템은 상호작용하는 즉시 효과를 일정 시간동안 받을 수 있습니다. 무적이나 이동 속도 증가 효과를 부여합니다.

아이템 데이터는 Sriptable Object로 관리했습니다.

### 맵 내 상호 작용

- 레이저 트랩: 레이저에 닿으면 데미지를 입습니다.
- 문: 클릭하여 상호작용할 수 있고 여닫을 수 있습니다.
- 벽: 지정된 Layer의 벽을 탈 수 있습니다. 벽에 닿아 바라보면서 W를 누르면 올라갈 수 있습니다.

## 트러블 슈팅

### 즉발 아이템 효 해제

**문제**

- 아이템 효과가 일정 시간 후에 해제 되어야 하는데 해제가 되지 않는다.

```csharp
IEnumerator ApplyItem(ItemData item)
{
    switch (item.active.type)
    {
        case ActiveType.Speed:
            CharacterManager.Instance.Player.controller.moveSpeed += item.active.value;
            break;
        case ActiveType.Invincible:
            CharacterManager.Instance.Player.condition.invincible = true;
            break;
    }

    yield return new WaitForSeconds(item.active.duration);

    switch (item.active.type)
    {
        case ActiveType.Speed:
            CharacterManager.Instance.Player.controller.moveSpeed -= item.active.value;
            break;
        case ActiveType.Invincible:
            CharacterManager.Instance.Player.condition.invincible = false;
            break;
    }

}
```

**해결**

- 아이템을 상호 작용하면 아이템이 삭제되어서 다음 코드가 실행이 안됐었다.
- 아이템 실행 코드를 다른 스크립트로 옮겨 해결했다.

### 플랫폼 발사대

**문제**

- 플레이어 위치 조정하고 발사하는 것까지 코루틴으로 만들어 놨는데, 발사만 안된다..

```csharp
IEnumerator LaunchCo(Rigidbody rb, Transform player)
{
    player.position = transform.position;
    rb.isKinematic = true;
    player.SetParent(transform);

    Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

    float elapsedTime = 0f;
    while(elapsedTime < 2f)
    {
        elapsedTime += Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, elapsedTime);
        yield return null;

    }

    yield return new WaitForSeconds(delay);

    rb.isKinematic = false;
    Vector3 launchDirection = transform.up;
    rb.AddForce(launchDirection * launchPower, ForceMode.Impulse);
    Debug.Log(rb.velocity);
    player.SetParent(null);

}
```

**해결**

- isKinematic = false로 했는데도 바로 적용이 안돼서 AddForce가 안됐었다.
- 위치 고정하는 부분은 없앴다.

**문제**

- 발사대에 탄 플레이어가 계속 위로만 발사된다.

**해결**

- PlayerController의 FixedUpdate에서 Move() 때문이었다.
- Move()를 특정 조건에서만 실행되게 바꾸어 해결하였다.

### 인벤토리 내 아이템 선택

**문제**

- 스크롤하여 인벤토리 내 아이템을 선택할 수 있게 했는데, 휠을 한 번 내려도 두 번 이상 슬롯의 인덱스가 이동했다.

```csharp
public void OnScroll(InputAction.CallbackContext context)
 {
     if (hasItem)
     {
         if (context.phase != InputActionPhase.Performed) return;
         float scrollValue = context.ReadValue<Vector2>().y;

         if (scrollValue > 0)
         {
             ChangeSelectedItem(1);
         }
         else if (scrollValue < 0)
         {
             ChangeSelectedItem(-1);
         }
     }
 }
```

**해결**

- Performed 조건을 추가해 중복 입력을 방지해줬다.

## 라이선스

| 에셋 | 출처 | 라이선스 |
| --- | --- | --- |
| POLY - Lite Survival Collection | [https://assetstore.unity.com/packages/3d/props/poly-lite-survival-collection-220452](https://assetstore.unity.com/packages/3d/props/poly-lite-survival-collection-220452) | MIT License |
| DoTween | [https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676](https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676) | MIT License |
| Low Poly Survival Modular Kit VR and  Mobile | [https://assetstore.unity.com/packages/3d/environments/low-poly-survival-modular-kit-vr-and-mobile-128903](https://assetstore.unity.com/packages/3d/environments/low-poly-survival-modular-kit-vr-and-mobile-128903) | MIT License |