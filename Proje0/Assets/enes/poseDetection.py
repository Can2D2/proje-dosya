import cv2
import base64
import mediapipe as mp
import numpy as np
import socket
import json

mp_drawing = mp.solutions.drawing_utils
mp_pose = mp.solutions.pose

def get_landmark_coordinates(landmarks, pose_landmark):
    return [landmarks[pose_landmark.value].x, landmarks[pose_landmark.value].y]

def get_coordinates(landmarks, body_part):
    return get_landmark_coordinates(landmarks, getattr(mp_pose.PoseLandmark, body_part))

def calculate_angle(a, b, c):
    a = np.array(a)
    b = np.array(b)
    c = np.array(c)

    radians = np.arctan2(c[1]-b[1], c[0]-b[0]) - np.arctan2(a[1]-b[1], a[0]-b[0])
    angle = np.abs(radians * 180.0 / np.pi)

    if angle > 180.0:
        angle = 360 - angle

    return angle

cap = cv2.VideoCapture(0)

cap.set(cv2.CAP_PROP_FRAME_WIDTH, 640)
cap.set(cv2.CAP_PROP_FRAME_HEIGHT, 480)

with mp_pose.Pose(min_detection_confidence=0.5, min_tracking_confidence=0.5) as pose:
    while cap.isOpened():
        ret, frame = cap.read()

        image = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
        image.flags.writeable = False

        results = pose.process(image)

        image.flags.writeable = True
        image = cv2.cvtColor(image, cv2.COLOR_RGB2BGR)

        try:
            landmarks = results.pose_landmarks.landmark

            l_shoulder= get_coordinates(landmarks, "LEFT_SHOULDER")
            l_elbow= get_coordinates(landmarks, "LEFT_ELBOW")
            l_wrist= get_coordinates(landmarks, "LEFT_WRIST")
            l_hip= get_coordinates(landmarks, "LEFT_HIP")
            l_knee= get_coordinates(landmarks, "LEFT_KNEE")
            l_ankle= get_coordinates(landmarks, "LEFT_ANKLE")
            r_shoulder= get_coordinates(landmarks, "RIGHT_SHOULDER")
            r_elbow= get_coordinates(landmarks, "RIGHT_ELBOW")
            r_wrist= get_coordinates(landmarks, "RIGHT_WRIST")
            r_hip= get_coordinates(landmarks, "RIGHT_HIP")
            r_knee= get_coordinates(landmarks, "RIGHT_KNEE")
            r_ankle= get_coordinates(landmarks, "RIGHT_ANKLE")

            coordinates = [l_shoulder, r_shoulder ,l_elbow, r_elbow ,l_wrist, r_wrist,
                           l_hip, r_hip, l_knee, r_knee, l_ankle, r_ankle]

            angles = [int(calculate_angle(l_hip, l_shoulder, l_elbow)), 
                      int(calculate_angle(r_hip, r_shoulder, r_elbow)), 
                      int(calculate_angle(l_shoulder, l_elbow, l_wrist)), 
                      int(calculate_angle(r_shoulder, r_elbow, r_wrist)),
                      int(calculate_angle(l_knee, l_hip, l_shoulder)), 
                      int(calculate_angle(r_knee, r_hip, r_shoulder)),
                      int(calculate_angle(l_ankle, l_knee, l_hip)), 
                      int(calculate_angle(r_ankle, r_knee, r_hip))]
            sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
        
            serverAddressPortImg = ("127.0.0.1", 1012)
            serverAddressPortCor = ("127.0.0.1", 2024)
            serverAddressPortAng = ("127.0.0.1", 3036)

            encode_param = [int(cv2.IMWRITE_JPEG_QUALITY), 16]
            _, img_encoded = cv2.imencode('.jpeg', image, encode_param)
            img_base64 = base64.b64encode(img_encoded).decode('utf-8')

            coordinates_json = json.dumps(coordinates)
            angles_json = json.dumps(angles)

            sock.sendto(img_base64.encode('utf-8'), serverAddressPortImg)
            sock.sendto(coordinates_json.encode('utf-8'), serverAddressPortCor)
            sock.sendto(angles_json.encode('utf-8'), serverAddressPortAng)
            
            #cv2.putText(image, str(angle),
                        #tuple(np.multiply(mid, [640, 480]).astype(int)),
                        #cv2.FONT_HERSHEY_SIMPLEX, 0.5, (255, 255, 255), 2, cv2.LINE_AA)

            mp_drawing.draw_landmarks(image, results.pose_landmarks, mp_pose.POSE_CONNECTIONS,
                                mp_drawing.DrawingSpec(color=(245,117,66), thickness=2, circle_radius=2), 
                                mp_drawing.DrawingSpec(color=(245,66,230), thickness=2, circle_radius=2) 
                                 )
            
            cv2.imshow("Mediapipe feed", image)

        except Exception as e:
            print(f"Error processing frame: {e}")

        if cv2.waitKey(10) & 0xFF == ord('q'):
            break

    cap.release()
    cv2.destroyAllWindows()

    #l shoulder
    #r shoulder
    #l elbow
    #r elbow
    #l wrist !
    #r wrist !
    #l hip
    #r hip
    #l knee
    #r knee
    #l ankle !
    #r ankle !
    #coordinates ve angles arraylerindeki sıra (yukarıdan aşağıya doğru 0, 1 ...)
    #yanında ! olanlar coordinatesta var anglesta yok