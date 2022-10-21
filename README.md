![header](https://capsule-render.vercel.app/api?type=waving&color=auto&height=200&section=header&text=Gojong&fontSize=60)

# Gojong

### 프로젝트 기간
2022.04 ~ 2022.06

### 프로젝트 내용
Google Dialogflow를 이용하며 스트리밍 기능을 추가해 이전 대화형 AI 김구보다 더 빠르고 정확하게 커뮤니케이션이 가능한 프로젝트입니다.
현재 덕수궁에서 전시되고 있습니다.\
<img width="40%" src="https://user-images.githubusercontent.com/90584581/197148019-b361aece-da6c-49f7-8872-4851f89203cf.png"/>  <img width="17%" src="https://user-images.githubusercontent.com/90584581/197148038-080f5a5f-27cb-47c8-af24-f15941264b1f.jpg"/>  <img width="13%" src="https://user-images.githubusercontent.com/90584581/197148028-b82ec493-b1a3-4b1f-9a63-b85b793a1828.png"/>

### 프로젝트 투입 인원
개발자 2, 디자이너 1

### 나의 역할
- Google Dialogflow API 연동
- 코드 결합 및 수정
- 스트리밍 기능 추가
- 코드 리팩터링 및 최적화
- 프로젝트 QA
- 프로젝트 유지보수
- 클라이언트와 지속적으로 면담 및 요구사항 수정

### 핵심 코드
서버와 클라이언트를 따로 두고 서버에서 응답을 받고 클라이언트에게 전달하는 방식으로 프로젝트가 진행되었습니다.

서버와 연결되는 클라이언트 코드

        ```
        public void ConnectedToServer()
        {
            if (socketReady) return;

            string ip = IPInput.text == "" ? "127.0.0.1" : IPInput.text;
            int port = PortInput.text == "" ? 7777 : int.Parse(PortInput.text);

            try
            {
                socket = new TcpClient(ip, port);
                stream = socket.GetStream();
                writer = new StreamWriter(stream);
                reader = new StreamReader(stream);
                socketReady = true;
            }
            catch (Exception e)
            {
                Chat.instance.ShowMessage($"소켓에러 : {e.Message}");
            }
        }
        ```
받아온 응답을 기준으로 애니메이션과 오디오 재생하는 코드

        ```
        public void ShowMessage(string data)
        {
            Debug.Log(data);
            if (data.Contains(":"))
            {
                string[] dataArr = data.Split(':');

                dataNum = int.Parse(dataArr[1].Trim());
                Debug.Log(dataNum);
                ChatText.text += ChatText.text == "" ? data : "\n" + data;
            }

            if(dataNum == 1000) // 사람 없을 때
                state = 1000;

            else if(dataNum == 2000) // 사람 있을 때
                state = 2000;

            if(state == 1000)
                dataNum = 0;

            else if(state == 2000) 
            {
                if (!df2.isAnimation) 
                { 
                    df2.playAnimation(dataNum);                
                }
            }
            Fit(ChatText.GetComponent<RectTransform>());
            Fit(ChatContent);
            Invoke("ScrollDelay", 0.03f);
        }
        ```
![Footer](https://capsule-render.vercel.app/api?type=waving&color=auto&height=200&section=footer)
