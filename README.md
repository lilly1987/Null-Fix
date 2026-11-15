# Null Fix

RimWorld 1.7 Harmony patch that skips invalid `Thing` arguments in `Gravship.AddThing`. It also guards the pawn carry-drop path used by `AddThing`: if `TryDropCarriedThing` reports success but returns a null result, it is treated as an unsuccessful drop so the game does not dereference a null result. This narrow guard applies only while `Gravship.AddThing` is running.

`Gravship.AddThing`에 유효하지 않은 `Thing`이 전달되면 해당 호출을 건너뛰는 RimWorld 1.7용 Harmony 패치입니다. `AddThing`에서 Pawn이 든 물건을 떨어뜨리는 과정 중 `TryDropCarriedThing`이 성공을 반환하면서 결과는 null인 경우, 해당 드롭을 실패로 처리하여 null 역참조를 막습니다. 이 가드는 `Gravship.AddThing` 실행 중에만 적용됩니다.

Requires Harmony. Build with `Build.bat`.
