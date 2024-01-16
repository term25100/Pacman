import sqlite3
from tkinter import*
from tkinter import filedialog as fd
from tkinter.filedialog import askdirectory
from tkinter import messagebox

def NewDir():
    global dir_name
    dir_name= askdirectory()
    dir1= Label(text='Путь к файлу: ', justify=CENTER)
    dir1.grid(row=0, column=0, sticky="w", pady=10)
    dir2 = Label(textvariable=dir3, justify=CENTER)
    dir2.grid(row=0, column=1, sticky="w", pady=10)
    dir3.set(dir_name)

    base=Label(text='База данных:(*.an)', justify=CENTER)
    base.grid(row=1, column=0, sticky="w", pady=10)
    base_name=Entry(textvariable=fn)
    base_name.grid(row=1, column=1, sticky="w", pady=10)

    b1=Button(text="Cоздать", command=Create)
    b1.grid(row=2, column=0, sticky=E, pady=10)

def Create():
    global file_name
    file_name=dir_name+'/'+fn.get()+'.an'
    conn=sqlite3.connect(file_name)
    cursor=conn.cursor()
    base5=Label(text=file_name, justify=CENTER)
    base5.grid(row=5, column=0, sticky="w", columnspan=22)

