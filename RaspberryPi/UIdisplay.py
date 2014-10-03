# -*- coding: utf-8 -*-

# Form implementation generated from reading ui file 'C:/Alex/Dropbox/Calexo/0Projets/SideScreen/SideScreen_RpiApp/display.ui'
#
# Created: Mon Sep 29 17:01:49 2014
#      by: PyQt4 UI code generator 4.11.2
#
# WARNING! All changes made in this file will be lost!

from PyQt4 import QtCore, QtGui

try:
    _fromUtf8 = QtCore.QString.fromUtf8
except AttributeError:
    def _fromUtf8(s):
        return s

try:
    _encoding = QtGui.QApplication.UnicodeUTF8
    def _translate(context, text, disambig):
        return QtGui.QApplication.translate(context, text, disambig, _encoding)
except AttributeError:
    def _translate(context, text, disambig):
        return QtGui.QApplication.translate(context, text, disambig)

class Ui_DisplayWindow(object):
    def setupUi(self, DisplayWindow):
        DisplayWindow.setObjectName(_fromUtf8("DisplayWindow"))
        DisplayWindow.resize(744, 1000)
        self.centralwidget = QtGui.QWidget(DisplayWindow)
        self.centralwidget.setObjectName(_fromUtf8("centralwidget"))
        self.verticalLayout_2 = QtGui.QVBoxLayout(self.centralwidget)
        self.verticalLayout_2.setObjectName(_fromUtf8("verticalLayout_2"))
        self.picBig = QtGui.QLabel(self.centralwidget)
        self.picBig.setAlignment(QtCore.Qt.AlignCenter)
        self.picBig.setObjectName(_fromUtf8("picBig"))
        self.verticalLayout_2.addWidget(self.picBig)
        DisplayWindow.setCentralWidget(self.centralwidget)
        self.menubar = QtGui.QMenuBar(DisplayWindow)
        self.menubar.setGeometry(QtCore.QRect(0, 0, 744, 26))
        self.menubar.setObjectName(_fromUtf8("menubar"))
        DisplayWindow.setMenuBar(self.menubar)
        self.statusbar = QtGui.QStatusBar(DisplayWindow)
        self.statusbar.setObjectName(_fromUtf8("statusbar"))
        DisplayWindow.setStatusBar(self.statusbar)

        self.retranslateUi(DisplayWindow)
        QtCore.QMetaObject.connectSlotsByName(DisplayWindow)

    def retranslateUi(self, DisplayWindow):
        DisplayWindow.setWindowTitle(_translate("DisplayWindow", "Display", None))
        self.picBig.setText(_translate("DisplayWindow", "picBig", None))

