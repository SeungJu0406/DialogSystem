# DialogSystem

# 🗨️ NSJ Dialogue System — R&D

> Unity 기반 서브컬쳐 장르를 위한 데이터 주도 대화 시스템 연구 프로젝트
> 현재 진행 중, 구조 설계와 확장성 검증에 초점을 맞춤

---

## 🔍 개요

이 프로젝트는 **UI 슬롯을 Label 기반으로 추상화**하여, 코드 수정 없이 다양한 UI 구성에 대응할 수 있는 구조를 목표

![녹음 2026-04-05 235311](https://github.com/user-attachments/assets/592ef14b-f738-42df-9925-e2a58fa898f2)

---

## 🏗️ 구조

```
NSJ_Dialogue
├── DialogueSystem          # 싱글톤 핵심 엔진 (대화 상태 관리, 분기 처리)
├── DialogueManager         # 외부 접근용 static Facade
├── DialogueRunner          # MonoBehaviour 진입점
├── DialogueScript (SO)     # 대화 데이터 (CSV 또는 직접 입력)
├── DialogueHeader (SO)     # CSV 컬럼 타입 정의
├── DialogueDisplayContainer # Label 기반 UI 슬롯 관리
└── DialogueDisplaySlot<T>  # 제네릭 UI 슬롯 (Text / Image / Choice)

Utility
└── SingleTon<T>            # 제네릭 싱글톤 베이스

ResourceFolder
└── ResourcesContainer      # Addressables 기반 스프라이트 로더
```
<img width="176" height="229" alt="화면 캡처 2026-04-06 000346" src="https://github.com/user-attachments/assets/24dc5e68-292c-451f-9d8e-11757b5e4467" />

---

## ✨ 핵심 설계 포인트

### 1. 제네릭 디스플레이 슬롯
```csharp
DialogueDisplaySlot<TMP_Text>   // 텍스트 슬롯
DialogueDisplaySlot<Image>      // 이미지 슬롯
DialogueDisplaySlot<Transform>  // 선택지 레이아웃 슬롯
```
UI 컴포넌트 타입에 관계없이 동일한 인터페이스
새로운 슬롯 타입 추가 시 기존 코드를 수정할 필요 X

### 2. Label 기반 슬롯 연결
인스펙터에서 Label만 맞춰주면 어떤 UI 구성이든 연결
```
"Name"      → TMP_Text  (캐릭터 이름)
"Text"      → TMP_Text  (대화 본문)
"FaceImage" → Image     (캐릭터 이미지)
"Choice"    → Transform (선택지 레이아웃)
```
<img width="455" height="447" alt="화면 캡처 2026-04-05 235405" src="https://github.com/user-attachments/assets/b1da3169-eabf-4567-992a-fc1dc8ee0932" />
<img width="247" height="178" alt="화면 캡처 2026-04-05 235521" src="https://github.com/user-attachments/assets/08bcc5ed-c44d-405c-baf2-49960983e498" />


### 3. 데이터 주도 설계 (CSV 연동)
기획자가 Unity 에디터 없이 스프레드시트로 대화를 작성하고 관리 가능
CSV 컬럼 구조 자체를 ScriptableObject로 정의하여, 포맷이 바뀌어도 코드 수정이 불필요함

- csv연동
<img width="454" height="297" alt="화면 캡처 2026-04-05 235908" src="https://github.com/user-attachments/assets/52330449-5c8c-4833-9ef5-25a6e25f366a" />
<img width="453" height="338" alt="화면 캡처 2026-04-06 000106" src="https://github.com/user-attachments/assets/b0df9a4c-de80-4126-9ce3-708ef582fc22" />


### 4. ID 기반 분기
```
ID:1  Aran / "Hello"
  ├─ Choice1 → ID:5   Aran / "What"  → ID:7
  └─ Choice2 → ID:6   Evan / "LOL"   → ID:7
                            ↓
                ID:7  Evan / "Sry :p"   ← 분기 합류
```

---

## 📁 데이터 구조

### Dialogue Line
```csharp
DialogueLine
├── ID              // 이 라인의 식별자
├── NextID          // 다음 라인 ID (없으면 순차 진행)
├── TextElements    // [ { Label, Text }, ... ]
├── ImageElements   // [ { Label, SpriteName }, ... ]
└── ChoiceElements  // [ { Label, ChoiceText, NextID }, ... ]
```
<img width="445" height="515" alt="화면 캡처 2026-04-06 000450" src="https://github.com/user-attachments/assets/9b41b54a-b915-49c7-941e-2b376eba1591" />

---

## 🔧 현재 구현 상태

| 기능 | 상태 |
|---|---|
| CSV 기반 대화 데이터 로드 | ✅ 완료 |
| ID 기반 분기 (선택지) | ✅ 완료 |
| 제네릭 UI 슬롯 시스템 | ✅ 완료 |
| Addressables 스프라이트 로드 | ✅ 완료 |
| Label 기반 슬롯 연결 | ✅ 완료 |

---

> 📌 본 프로젝트는 포트폴리오가 아닌 **개인 R&D 목적**으로 진행 중입니다.
