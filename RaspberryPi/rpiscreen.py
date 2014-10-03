#!/usr/bin/python
import sys
import socket
import threading
import os

from PyQt4.QtGui import QApplication, QDialog, QMainWindow
from UIrpiscreen import Ui_RpiScreen
from UIdisplay import Ui_DisplayWindow
from PyQt4.QtCore import QObject, pyqtSignal
from PyQt4 import QtCore as qtcore
from PyQt4 import QtGui as QtGui

from socket import error as SocketError
import errno


BUFF = 1024
HOST = '0.0.0.0'
PORT = 5024

class MainWindow(QMainWindow, Ui_RpiScreen):
 
    # custom slot
    # def mymethod(self):
        # self.textFieldExample.setText('Hello World')
        # self.textFieldExample.clear()
        # 
    sigMemChanged = pyqtSignal(int, int) # Free, Total
    sigProcChanged = pyqtSignal(int, int) # Proc# et #%

    menu = []
    dossier = ""

    

 
    def __init__(self):
        QMainWindow.__init__(self)
 
       # set up User Interface (widgets, layout...)
        self.setupUi(self)

        self.pics = [self.pic7, self.pic8, self.pic9, self.pic4, self.pic5, self.pic6, self.pic1, self.pic2, self.pic3]
        self.pbProcs = [self.pbProc1, self.pbProc2, self.pbProc3, self.pbProc4, self.pbProc5, self.pbProc6, self.pbProc7, self.pbProc8]
        for i in range(1,8,1):
            self.pbProcs[i].setVisible(False)
        # self.pbProc2.setVisible(False)
        # self.pbProc3.setVisible(False)
        # self.pbProc4.setVisible(False)
        # self.pbProc5.setVisible(False)
        # self.pbProc6.setVisible(False)
        # self.pbProc7.setVisible(False)
        # self.pbProc8.setVisible(False)
        self.connectActions()

        self.loadMenu(".")

        self.serversocket = socket.socket(socket.AF_INET, socket.SOCK_STREAM) 


    def loadMenu(self, dossier):
        for i in range(0,9,1):
            self.pics[i].setPixmap(QtGui.QPixmap())
        # self.pic7.setPixmap(QtGui.QPixmap())
        # self.pic8.setPixmap(QtGui.QPixmap())
        # self.pic9.setPixmap(QtGui.QPixmap())
        i=0
        self.menu=[]
        for dirname, dirnames, filenames in os.walk("/home/pi/rpiside/data/"+dossier):
            for filename in filenames:
                print ( filename )
                myPixmap = QtGui.QPixmap('/home/pi/rpiside/data/'+dossier+'/'+filename)
                # if i==0:
                #     currentPic = self.pic7
                # if i==1:
                #     currentPic = self.pic8
                # if i==2:
                #     currentPic = self.pic9
                if i<9:
                    currentPic = self.pics[i]
                    self.menu.append(os.path.splitext(filename)[0])
                    # myScaledPixmap = myPixmap.scaled(currentPic.size(), qtcore.Qt.IgnoreAspectRatio)
                    myScaledPixmap = myPixmap.scaled(self.pic7.size(), qtcore.Qt.KeepAspectRatio)
                    currentPic.setPixmap(myScaledPixmap)
                i+=1
        print ("I=" + str(i))
        if i==0: # Pas de dossier -> afficher l'image
            self.lblInfo.setText("Displaying...")
            self.bigPic = DisplayWindow()
            self.bigPic.showMaximized()
            myPixmap = QtGui.QPixmap('/home/pi/rpiside/data/'+dossier)
            myScaledPixmap = myPixmap.scaled(self.bigPic.picBig.size(), qtcore.Qt.KeepAspectRatio)
            self.bigPic.picBig.setPixmap(myScaledPixmap)


    def keyPressEvent(self, event):
        # http://pyqt.sourceforge.net/Docs/PyQt4/qt.html#Key-enum
        key = event.key()
        if key == qtcore.Qt.Key_7:
            newmenu=self.menu[0]
        elif key == qtcore.Qt.Key_8:
            newmenu=self.menu[1]
        elif key == qtcore.Qt.Key_9:
            newmenu=self.menu[2]
        elif key == qtcore.Qt.Key_4:
            newmenu=self.menu[3]
        elif key == qtcore.Qt.Key_5:
            newmenu=self.menu[4]
        elif key == qtcore.Qt.Key_6:
            newmenu=self.menu[5]
        elif key == qtcore.Qt.Key_1:
            newmenu=self.menu[6]
        elif key == qtcore.Qt.Key_2:
            newmenu=self.menu[7]
        elif key == qtcore.Qt.Key_3:
            newmenu=self.menu[8]
        elif key == qtcore.Qt.Key_0:
            # On remonte
            self.dossier = self.dossier.rsplit('/',1)[0].rstrip('/')
            newmenu=""
        else:
            self.lblInfo.setText(str(key))

        self.lblInfo.setText(newmenu)
        if len(newmenu)>0:
            self.dossier= self.dossier + '/'+newmenu
        self.lblInfo.setText(self.dossier)
        self.loadMenu(self.dossier)
        

    def closeEvent(self, event):
        self.serversocket.close()

    def updateProc(self, procNum, procUse):
        if procNum<99:
            self.pbProcs[procNum].setProperty("value",procUse)
            self.pbProcs[procNum].setVisible(True)
        if 99==procNum:
            self.lcdMoyProc.setProperty("intValue",procUse)
    def updateMem(self, FreeMem, TotalMem):
        print("Sig updateMem")
        if (FreeMem>0):
            print("Sig -> Update FreeMem")
            self.pbMem.setProperty("value", FreeMem)
            self.lcdFreeMem.setProperty("intValue",FreeMem)
        if (TotalMem>0):
            print("Sig -> Update TotalMem")
            self.pbMem.setProperty("maximum", TotalMem)

    def updateVol(self, vol):
        print("Sig updateVol")
        self.dialVolume.setProperty("value", vol)


    def connectActions(self):
        self.action_Exit.triggered.connect(app.quit)
        self.connect (self, qtcore.SIGNAL("sigMemChanged"), self.updateMem )
        self.connect (self, qtcore.SIGNAL("sigProcChanged"), self.updateProc )
        self.connect (self, qtcore.SIGNAL("sigVolChanged"), self.updateVol )

    def listen(self):
        
        self.serversocket.bind((HOST, PORT)) 
        self.serversocket.listen(5)
        # while True:
            # establish a connection
        while True:
            try:
                clientsocket,addr = self.serversocket.accept()      

                print("Got a connection from %s" % str(addr))
                # currentTime = time.ctime(time.time()) + "\r\n"
                newinfo = clientsocket.recv(1024) 
                clientsocket.send("OK".encode("ascii"))
                # clientsocket.close()

                print("The info is %s" % newinfo.decode('ascii'))

                newinfos=newinfo.split()
                type_info=newinfos[0].decode('ascii')
                value_arr=newinfos[1].decode('ascii').split('.')
                value_info = value_arr[0]


                print ( newinfos[0] ," = ", value_info)

                print ( type_info ," = ", value_info)

                if "FreeMem" == type_info:
                    print("FM")
                    # window.pbMem.setProperty("value", int(value_info))
                    self.emit(qtcore.SIGNAL("sigMemChanged"),int(value_info),0)
                if "TotalMem" == type_info:
                    print("TM")
                    # window.pbMem.setProperty("maximum", int(value_info))
                    self.emit(qtcore.SIGNAL("sigMemChanged"),0,int(value_info))
                if "Proc1" == type_info:
                    print("P1")
                    self.emit(qtcore.SIGNAL("sigProcChanged"),0,int(value_info))
                if "Proc2" == type_info:
                    print("P2")
                    self.emit(qtcore.SIGNAL("sigProcChanged"),1,int(value_info))
                if "Proc3" == type_info:
                    print("P3")
                    self.emit(qtcore.SIGNAL("sigProcChanged"),2,int(value_info))
                if "Proc4" == type_info:
                    print("P4")
                    self.emit(qtcore.SIGNAL("sigProcChanged"),3,int(value_info))
                if "Proc5" == type_info:
                    print("P5")
                    self.emit(qtcore.SIGNAL("sigProcChanged"),3,int(value_info))
                if "Proc6" == type_info:
                    print("P6")
                    self.emit(qtcore.SIGNAL("sigProcChanged"),3,int(value_info))
                if "Proc7" == type_info:
                    print("P7")
                    self.emit(qtcore.SIGNAL("sigProcChanged"),3,int(value_info))
                if "Proc8" == type_info:
                    print("P8")
                    self.emit(qtcore.SIGNAL("sigProcChanged"),3,int(value_info))
                if "Procs" == type_info:
                    print("P*")
                    self.emit(qtcore.SIGNAL("sigProcChanged"),99,int(value_info))
                if "Vol" == type_info:
                    print("Vol")
                    self.emit(qtcore.SIGNAL("sigVolChanged"),int(value_info))

                # window.pbProc1.setProperty("value", int(value_info))

            except SocketError as e:
                if e.errno != errno.ECONNRESET:
                    print("Fin server socket : ", sys.exc_info()[0])
                    print("Fin server socket : ", sys.exc_info()[1])
                    raise # Not error we are looking for
                print ("Connection reset by peer... Continue.")
                pass # Handle error here.

class DisplayWindow(QMainWindow, Ui_DisplayWindow):
    def __init__(self):
        QMainWindow.__init__(self)
       # set up User Interface (widgets, layout...)
        self.setupUi(self)    
    def keyPressEvent(self, event):
        # http://pyqt.sourceforge.net/Docs/PyQt4/qt.html#Key-enum
        key = event.key()
        if key == qtcore.Qt.Key_4:
            # newmenu=self.menu[3]
            print("Gauche")
        elif key == qtcore.Qt.Key_6:
            print("Droite")
            # newmenu=self.menu[5]
        elif key == qtcore.Qt.Key_0:
            # On remonte
            window.dossier = window.dossier.rsplit('/',1)[0].rstrip('/')
            newmenu=""
            self.close()
        else:
            window.lblInfo.setText(str(key))

        window.lblInfo.setText(newmenu)
        if len(newmenu)>0:
            window.dossier= window.dossier + '/'+newmenu
        window.lblInfo.setText(window.dossier)
        window.loadMenu(window.dossier)

app = QApplication(sys.argv)
window = MainWindow()
# ui = Ui_RpiScreen()
# ui.setupUi(window)

window.show()
# window.showFullScreen()
window.showMaximized()

# thread.start_new_thread( listen )
threading.Thread(target=window.listen).start()


sys.exit(app.exec_())


